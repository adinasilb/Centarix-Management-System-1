using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PrototypeWithAuth.ViewModels;

namespace PrototypeWithAuth.CRUD
{
    public class TemporaryLocationInstancesProc : ApplicationDbContextProc<TemporaryLocationInstance>
    {
        public TemporaryLocationInstancesProc(ApplicationDbContext context, bool FromBase = false) : base(context)
        {
            if (!FromBase)
            {
                base.InstantiateProcs();
            }
        }

        public async Task CreateWithoutTransactionAsync(TemporaryLocationInstance temporaryLocationInstance, int locationTypeID)
        {
            if (temporaryLocationInstance == null)
            {
                var locationTypeName = _locationTypesProc.ReadOneAsync(new List<Expression<Func<LocationType, bool>>> { lt => lt.LocationTypeID == locationTypeID}).Result.LocationTypeName;
                temporaryLocationInstance = new TemporaryLocationInstance()
                {
                    LocationTypeID = locationTypeID,
                    LocationInstanceName = "Temporary " + locationTypeName,
                    LocationInstanceAbbrev = "Temporary " + locationTypeName
                };
                _context.Entry(temporaryLocationInstance).State = EntityState.Added;
                await _context.SaveChangesAsync();
            }
        }

    }
}
