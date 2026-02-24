using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;

using JohaEfCrypter.Attributes;
using JohaEfCrypter.Extensions;

using Microsoft.EntityFrameworkCore.Diagnostics;

namespace JohaEfCrypter.Intecepters
{
    public class DecryptioInterceptor : IMaterializationInterceptor
    {

        public object InitializedInstance(MaterializationInterceptionData materializationData, object entity)
        {
            var encryptTable = entity.GetType().GetCustomAttribute<EncryptTableAttribute>();
            if (encryptTable != null)
            {
                DecryptEntity(entity);
            }

            return entity;
        }
        PropertyInfo CheckProp(IEnumerable<PropertyInfo> props)
        {
            var checkSum = props.FirstOrDefault(m => m.GetCustomAttribute<EncryptedAttribute>().CheckSum);
            return checkSum;
        }
        static string GetName( PropertyInfo info)
        {
            var attr = info.GetCustomAttribute<ColumnAttribute>();
            if (attr != null) return attr.Name.ToLower();
            return info.Name.ToLower();
        }
        private void DecryptEntity(object entity)
        {
            var props = entity.GetType().GetProperties()
                .Where(p => Attribute.IsDefined(p, typeof(EncryptedAttribute)));
            var checkSumProp = CheckProp(props);

            if (checkSumProp == null || string.IsNullOrEmpty((string)checkSumProp.GetValue(entity) ?? string.Empty))
            {
                return;
            }
            StringBuilder builder = new StringBuilder();
            foreach (var prop in props.Where(m => m.GetCustomAttribute<EncryptedAttribute>().IsEncrypt).OrderBy(m => GetName(m)))
            {
                if (prop.GetValue(entity) is string value && !string.IsNullOrEmpty(value))
                {
                    var decValue = value.DecryptBase64();
                    var name = GetName(prop) + "|" + decValue + ";";
                    builder.Append(name);
                    prop.SetValue(entity, decValue);
                }
            }
            var hash = builder.ToString().HashString();
            var oldHash = checkSumProp.GetValue(entity);
            if (hash != oldHash)
            {
                Console.WriteLine("Consident");
            }
            else
            {
                Console.WriteLine("Not Consident");
            }
        }
    }
}
