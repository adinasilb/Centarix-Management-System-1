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
    public class CategoryTypesProc : ApplicationDbContextProc<CategoryType>
    {
        public CategoryTypesProc(ApplicationDbContext context, UserManager<ApplicationUser> userManager, bool FromBase = false) : base(context, userManager)
        {
            if (!FromBase) { this.InstantiateProcs(); }
        }

        public IQueryable<CategoryType> Read()
        {
            return _context.CategoryTypes.AsNoTracking().AsQueryable();
        }
    }
}
