using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PrototypeWithAuth.CRUD
{
    public class LocationRoomInstancesProc : ApplicationDbContextProc<LocationRoomInstance>
    {
        public LocationRoomInstancesProc(ApplicationDbContext context, bool FromBase = false) : base(context)
        {
            if (!FromBase)
            {
                base.InstantiateProcs();
            }
        }
    }
}
