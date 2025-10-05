using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interceptors
{
    public class EntitySaveChangesInterceptor
    {
    //    private readonly ICurrentUserService _currentUserService;

    //    public EntitySaveChangesInterceptor(ICurrentUserService currentUserService)
    //    {
    //        _currentUserService = currentUserService;
    //    }

    //    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    //    {
    //        UpdateEntities(eventData.Context);
    //        return base.SavingChanges(eventData, result);
    //    }

    //    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
    //        DbContextEventData eventData,
    //        InterceptionResult<int> result,
    //        CancellationToken cancellationToken = default)
    //    {
    //        UpdateEntities(eventData.Context);
    //        return base.SavingChangesAsync(eventData, result, cancellationToken);
    //    }

    //    private void UpdateEntities(DbContext context)
    //    {
    //        if (context == null) return;

    //        foreach (var entry in context.ChangeTracker.Entries<BaseAuditableEntity>())
    //        {
    //            if (entry.State == EntityState.Added)
    //            {
    //                entry.Entity.CreatedBy = _currentUserService.UserId;
    //                entry.Entity.CreatedDate = DateTime.UtcNow;
    //            }

    //            if (entry.State == EntityState.Modified)
    //            {
    //                entry.Entity.ModifiedBy = _currentUserService.UserId;
    //                entry.Entity.ModifiedDate = DateTime.UtcNow;
    //            }
    //        }
    //    }
}
}
