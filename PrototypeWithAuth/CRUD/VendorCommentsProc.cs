using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PrototypeWithAuth.CRUD
{
    public class VendorCommentsProc : ApplicationDbContextProc
    {
        public VendorCommentsProc(ApplicationDbContext context, UserManager<ApplicationUser> userManager, bool FromBase = false) : base (context, userManager)
        {
            if (!FromBase)
            {
                base.InstantiateProcs();
            }
        }

        public IQueryable<VendorComment> Read()
        {
            return _context.VendorComments.AsNoTracking().AsQueryable();
        }

        public IQueryable<VendorComment> ReadByVendorID(int VendorID, List<Expression<Func<VendorComment, object>>> includes = null)
        {
            var comments = _context.VendorComments
                .Where(vc => vc.ObjectID == VendorID);
            if (includes != null)
            {
                foreach (var t in includes)
                {
                    comments = comments.Include(t);
                }
            }
            return comments.AsNoTracking().AsQueryable();
        }
    }
}
