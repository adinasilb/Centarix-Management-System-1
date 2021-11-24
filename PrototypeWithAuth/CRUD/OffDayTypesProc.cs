using Microsoft.AspNetCore.Identity;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.CRUD
{
    public class OffDayTypesProc : ApplicationDbContextProcedure
    {
        public OffDayTypesProc(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : base (context, userManager)
        {

        }

        public IQueryable<OffDayType> ReadManyByPKS(List<int> PKs)
        {
            return _context.OffDayTypes.Where(od => PKs.Contains(od.OffDayTypeID));
        }
    }
}
