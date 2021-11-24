using Microsoft.AspNetCore.Identity;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.CRUD
{
    public class CompanyDaysOffProc : ApplicationDbContextProcedure
    {
        public CompanyDaysOffProc(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : base (context, userManager)
        {

        }

        public CompanyDayOff ReadOneByDate(DateTime date)
        {
            return _context.CompanyDayOffs.Where(cdo => cdo.Date.Date == date).FirstOrDefault();
        }
    }
}
