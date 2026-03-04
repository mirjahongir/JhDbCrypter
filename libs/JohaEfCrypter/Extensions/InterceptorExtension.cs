using JhCrypter.Attributes;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;

namespace JohaEfCrypter.Extensions
{
    public static class InterceptorExtension
    {

        internal static void ParseHash(EntityEntry entity, IEnumerable<PropertyEntry> props)
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
        internal static void HashingField(EntityEntry entity, IEnumerable<PropertyEntry> props)
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
        internal static string GetName(this PropertyEntry prop)
        {

            var attr = prop.Metadata.PropertyInfo.GetCustomAttribute<ColumnAttribute>();
            if (attr != null)
            {
                return attr.Name.ToLower();
            }
            return prop.Metadata.PropertyInfo.Name.ToLower();
        }
        public static PropertyEntry CheckProp(this IEnumerable<PropertyEntry> props)
        {
            var chechSum = props.FirstOrDefault(m => m.Metadata.PropertyInfo.GetCustomAttribute<EncryptedAttribute>().CheckSum);
            if (chechSum == null)
            {
                throw new Exception("Bu yerda tug`ri hato chiqarish kerak");
            }
            return chechSum;
        }
        public static PropertyInfo CheckProp(this IEnumerable<PropertyInfo> props)
        {
            var checkSum = props.FirstOrDefault(m => m.GetCustomAttribute<EncryptedAttribute>().CheckSum);
            return checkSum;
        }
        public static string GetName(this PropertyInfo info)
        {
            var attr = info.GetCustomAttribute<ColumnAttribute>();
            if (attr != null) return attr.Name.ToLower();
            return info.Name.ToLower();
        }
        public static Action<object>? ErrorAct { get; set; }
        public static void Error(object error)
        {
            if (ErrorAct != null)
            {
                ErrorAct(error);
            }
        }
    }
}
