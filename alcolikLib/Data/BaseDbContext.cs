using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using alcolikLib.Model;

namespace alcolikLib.Data
{
    public abstract class BaseDbContext : DbContext
    {
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            ChangeCreatedState();
            changeDeletedState();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public void ChangeCreatedState()
        {
            var createEntities = ChangeTracker.Entries().Where(x => x.State == EntityState.Added);
            foreach (var item in createEntities)
            {
                if (item.Entity is BaseModel model)
                {
                    model.Active = true;
                    model.CreatedAt = DateTime.Now;
                    model.DeletedAt = null;
                    /*
                    var entity = (BaseModel)item.Entity;
                    entity.Active = true;
                    */
                }

            }
        }

        public void changeDeletedState()
        {
            var deletedEntities = ChangeTracker.Entries().Where(x => x.State == EntityState.Deleted);
            foreach (var item in deletedEntities)
            {
                if (item.Entity is BaseModel model)
                {
                    model.Active = false;
                    model.DeletedAt = DateTime.Now;
                    item.State = EntityState.Modified;
                }
            }
        }
    }
}
