using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.CRUD
{
    public class EmployeeHoursStatuesProc : ApplicationDbContextProc<EmployeeHoursStatus>

    {
        public EmployeeHoursStatuesProc(ApplicationDbContext context, bool FromBase = false) : base (context)
        {
            if (!FromBase)
            {
                base.InstantiateProcs();
            }
        }

        public EmployeeHoursStatus ReadOneByPK(int? ID)
        {
            return _context.EmployeeHoursStatuses.Where(ehs => ehs.EmployeeHoursStatusID == ID)
                .AsNoTracking().FirstOrDefault();
        }
    }
}
