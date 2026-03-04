using JhCrypter.Attributes;
using JohaEfCrypter.Extensions;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JhCrypter;

namespace JohaEfCrypter.Intecepters
{
    public class CryptoInterceptor : SaveChangesInterceptor
    {
        public CryptoInterceptor() { }

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            var entities = eventData.Context
               .ChangeTracker
               .Entries()
               .Where(m => m.Entity.GetType().IsDefined(typeof(EncryptTableAttribute), inherit: true));
            foreach (EntityEntry entry in entities)
            {
                if (entry.State is (Microsoft.EntityFrameworkCore.EntityState.Added or Microsoft.EntityFrameworkCore.EntityState.Modified or Microsoft.EntityFrameworkCore.EntityState.Unchanged))
                {
                    EncryptEntity(entry);
                    //SaveInterceptor(entry);//
                }
            }
            return base.SavingChanges(eventData, result);
        }
        void EncryptEntity(EntityEntry entity)
        {
            var attr = entity.Entity.GetType().GetCustomAttribute<EncryptTableAttribute>();
            if (attr == null) { return; }
            var props = entity.Properties.Where(m =>
            m.Metadata.PropertyInfo?.GetCustomAttribute<EncryptedAttribute>() != null
            && m.Metadata.ClrType == typeof(string));

            ParseHash(entity, props);
            HashingField(entity, props);
            EncryptField(entity, props);
        }
        void ParseHash(EntityEntry entity, IEnumerable<PropertyEntry> props)
        {
            ArgumentNullException.ThrowIfNull(entity);
            foreach (var prop in props.Where(m => m.Metadata.PropertyInfo.GetCustomAttribute<EncryptedAttribute>().IsHash))
            {
                var value = prop.CurrentValue?.ToString();
                if (string.IsNullOrEmpty(value))
                {
                    continue;
                }
                if (value.StartsWith("hash"))
                {
                    continue;
                }
                prop.CurrentValue = value.HashString();
            }
        }
        void HashingField(EntityEntry entity, IEnumerable<PropertyEntry> props)
        {
            foreach (var prop in props.Where(m => !string.IsNullOrEmpty(m.Metadata.PropertyInfo.GetCustomAttribute<EncryptedAttribute>().HashField)))
            {
                var hashField = prop.Metadata.PropertyInfo.GetCustomAttribute<EncryptedAttribute>().HashField;
                var valueProp = entity.Entity.GetType().GetProperties().FirstOrDefault(m => string.Equals(hashField, m.Name, StringComparison.OrdinalIgnoreCase));
                var attr = valueProp.GetCustomAttribute<EncryptedAttribute>();
                if (attr != null)
                {
                    if (attr.IsHash)
                    {
                        //BUG:  textni tug`irlash kerak
                        throw new Exception("Property: " + prop.Metadata.Name + "  is Error ");
                    }
                }
                var realValue = valueProp?.GetValue(entity.Entity)?.ToString();
                if (string.IsNullOrEmpty(realValue)) continue;
                prop.CurrentValue = realValue?.HashString();
            }
        }
        void EncryptField(EntityEntry entity, IEnumerable<PropertyEntry> props)
        {
            ArgumentNullException.ThrowIfNull(entity);
            StringBuilder builder = new();
            var checkProp = props.CheckProp();
            if (checkProp == null) return;

            foreach (PropertyEntry prop in props
                                            .Where(m => m.Metadata.PropertyInfo.GetCustomAttribute<EncryptedAttribute>().IsEncrypt)
                                            .OrderBy(m => m.GetName()))
            {

                if (prop.CurrentValue is string str && !string.IsNullOrEmpty(str))
                {

                    var name = prop.GetName() + "|" + str + ";";
                    builder.Append(name);
                    var enct = str.EncryptStr();
                    prop.CurrentValue = enct;
                }
            }

            checkProp?.CurrentValue = builder.ToString().HashString();
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }
}
