using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PrototypeWithAuth.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.CRUD
{
    public class CategoryTypesProc : ApplicationDbContextProc
    {
        public CategoryTypesProc(ApplicationDbContext context, UserManager<ApplicationUser> userManager, bool FromBase = false) : base(context, userManager)
        {
            if (!FromBase) { this.InstantiateProcs(); }
        }

        public IQueryable<Models.CategoryType> Read()
        {
            return _context.CategoryTypes.AsNoTracking().AsQueryable();
        }
    }
}
