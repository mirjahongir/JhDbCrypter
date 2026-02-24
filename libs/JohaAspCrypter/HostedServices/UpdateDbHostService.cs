using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using JohaEfCrypter.Attributes;
using Microsoft.EntityFrameworkCore;

namespace JohaAspCrypter.HostedServices
{
    interface IWorker
    {
        Task StartAsync(CancellationToken token);
        Task StopAsync();
    }
    class UpdateDbHostService : IWorker
    {
        #region
        readonly DbContext _context;
        public UpdateDbHostService(DbContext context)
        {
            _context = context;
        }
        #endregion
        static IQueryable GetDbSet(DbContext db, Type entityType)
        {
            var method = typeof(DbContext)
                .GetMethod(nameof(DbContext.Set), Type.EmptyTypes)!;

            var generic = method.MakeGenericMethod(entityType);

            return (IQueryable)generic.Invoke(db, null)!;
        }
        static IQueryable BuildNullCheckQuery(DbContext db, Type entityType, string propertyName)
        {
            var set = GetDbSet(db, entityType);// db.Set(entityType);
            var parameter = Expression.Parameter(entityType, "x");
            var property = Expression.Property(parameter, propertyName);
            var body = Expression.Equal(
                property,
                Expression.Constant(null, property.Type)
            );

            var lambda = Expression.Lambda(body, parameter);
            var whereMethod = typeof(Queryable)
                                             .GetMethods()
                                             .Single(m =>
                                             {
                                                 if (m.Name != "Where") return false;
                                                 if (!m.IsGenericMethodDefinition) return false;

                                                 var parameters = m.GetParameters();
                                                 if (parameters.Length != 2) return false;

                                                 var parameterType = parameters[1].ParameterType;

                                                 // Expression<>
                                                 if (!parameterType.IsGenericType) return false;
                                                 if (parameterType.GetGenericTypeDefinition() != typeof(Expression<>))
                                                     return false;

                                                 // Func<,>
                                                 var funcType = parameterType.GetGenericArguments()[0];
                                                 if (!funcType.IsGenericType) return false;
                                                 if (funcType.GetGenericTypeDefinition() != typeof(Func<,>))
                                                     return false;

                                                 return true;
                                             })
                                             .MakeGenericMethod(entityType);
           
            return (IQueryable)whereMethod.Invoke(null, new object[] { set, lambda })!;
            
        }

        static string GetName(PropertyInfo info)
        {
            var attr = info.GetCustomAttribute<ColumnAttribute>();
            if (attr == null)
                return info.Name;
            return attr.Name;

        }
        public async Task StartAsync(CancellationToken token)
        {
            var encryptedEntities = _context.Model.GetEntityTypes().Where(e => e.ClrType.GetCustomAttribute<EncryptTableAttribute>() != null);
            foreach (var entityType in encryptedEntities)
            {
                var clrType = entityType.ClrType;
                var checkSumProps = clrType.GetProperties().Where(p => p.GetCustomAttribute<EncryptedAttribute>()?.CheckSum ?? false)
                                                            .FirstOrDefault();
                if (checkSumProps == null) continue;
                UpdateCheckSum(_context, clrType, checkSumProps);
            }
            return;

        }

        static void UpdateCheckSum(DbContext db, Type entityType, PropertyInfo checkSumProp)
        {
            var rows = BuildNullCheckQuery(db, entityType, GetName(checkSumProp));

            foreach (var entity in rows)
            {
                var value = "test";// ComputeCheckSum(entity);

                checkSumProp.SetValue(entity, value);

                db.Entry(entity).Property(checkSumProp.Name).IsModified = true;
            }

            db.SaveChanges();
        }

        public async Task StopAsync()
        {

        }
    }
}
