using System;
using System.Linq;
using System.Reflection;
using System.Text;

using JhCrypter.Attributes;

using JohaEfCrypter.Extensions;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace JohaEfCrypter.Intecepters
{
    public class DecryptioInterceptor : IMaterializationInterceptor
    {

        public DecryptioInterceptor() { }

        public object InitializedInstance(MaterializationInterceptionData materializationData, object entity)
        {
            var encryptTable = entity.GetType().GetCustomAttribute<EncryptTableAttribute>();
            if (encryptTable != null)
            {
                DecryptEntity(entity);
            }
            return entity;
        }

        private void DecryptEntity(object entity)
        {
            var props = entity.GetType().GetProperties()
                .Where(p => Attribute.IsDefined(p, typeof(EncryptedAttribute)));
            var checkSumProp = props.CheckProp();

            if (checkSumProp == null || string.IsNullOrEmpty((string)checkSumProp.GetValue(entity) ?? string.Empty))
            {
                return;
            }
            StringBuilder builder = new();
            foreach (var prop in props.Where(m => m.GetCustomAttribute<EncryptedAttribute>().IsEncrypt).OrderBy(m => m.GetName()))
            {
                if (prop.GetValue(entity) is string value && !string.IsNullOrEmpty(value))
                {
                    var decValue = value.DecryptBase64();
                    var name = prop.GetName() + "|" + decValue + ";";
                    builder.Append(name);
                    prop.SetValue(entity, decValue);
                }
            }
            var hash = builder.ToString().HashString();
            var oldHash = checkSumProp.GetValue(entity);
            if (hash != oldHash)
            {
                InterceptorExtension.Error(entity);
            }
        }
    }
}
