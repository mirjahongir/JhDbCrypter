using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JohaEfCrypter.Attributes;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace JohaEfCrypter.Extensions
{
    public static class InterceptorExtension
    {

        public static string GetName(this PropertyEntry prop)
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
        public static Action<object>? ErrorAct{ get; set; }
        public static void Error(object error)
        {
            if (ErrorAct != null)
            {
                ErrorAct(error);
            }
        }
    }
}
