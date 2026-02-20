using System;
using System.Linq;
using System.Reflection;
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

        private void DecryptEntity(object entity)
        {
            var props = entity.GetType().GetProperties()
                .Where(p => Attribute.IsDefined(p, typeof(EncryptedAttribute)));

            foreach (var prop in props.Where(m => m.GetCustomAttribute<EncryptedAttribute>().IsEncrypt))
            {
                if (prop.GetValue(entity) is string value && !string.IsNullOrEmpty(value))
                {
                    prop.SetValue(entity, value.DecryptBase64());
                }
            }
        }
    }
}
