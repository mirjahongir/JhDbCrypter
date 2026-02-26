using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JhCrypter.Attributes;
using JohaEfCrypter.Extensions;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace JohaEfCrypter.Intecepters
{
    public class CryptoInterceptor : SaveChangesInterceptor
    {
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
                    InterceptorExtension.EncryptEntity(entry);
                }
            }
            return base.SavingChanges(eventData, result);
        }
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }
}
