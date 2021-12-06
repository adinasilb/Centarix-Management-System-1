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
    public class EmployeeHoursStatuesProc : ApplicationDbContextProc
    {
        public EmployeeHoursStatuesProc(ApplicationDbContext context, UserManager<ApplicationUser> userManager, bool FromBase = false) : base (context, userManager)
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
