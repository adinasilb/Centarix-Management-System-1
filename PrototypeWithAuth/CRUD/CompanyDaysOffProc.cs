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
    public class CompanyDaysOffProc : ApplicationDbContextProc
    {
        public CompanyDaysOffProc(ApplicationDbContext context, UserManager<ApplicationUser> userManager, bool FromBase = false) : base (context, userManager)
        {
            if (!FromBase) { base.InstantiateProcs(); }
        }

        public CompanyDayOff ReadOneByDate(DateTime date)
        {
            return _context.CompanyDayOffs.Where(cdo => cdo.Date.Date == date).AsNoTracking().FirstOrDefault();
        }

        public IQueryable<CompanyDayOff> ReadByDateSpan(DateTime DateFrom, DateTime DateTo)
        {
            return _context.CompanyDayOffs.Where(d => d.Date >= DateFrom && d.Date <= DateTo).AsQueryable();
        }

        public IQueryable<CompanyDayOff> ReadByDate(DateTime date)
        {
            return _context.CompanyDayOffs.Where(cdo => cdo.Date.Date == date).AsQueryable();
        }

    }
}
