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
    public class VendorCommentProc : ApplicationDbContextProcedure
    {
        public VendorCommentProc(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : base (context, userManager)
        {

        }

        public IQueryable<VendorComment> Read()
        {
            return _context.VendorComments.AsNoTracking().AsQueryable();
        }

        public IQueryable<VendorComment> ReadByVendorID(int VendorID)
        {
            return _context.VendorComments.Include(vc => vc.ApplicationUser)
                .Where(vc => vc.VendorID == VendorID).AsNoTracking().AsQueryable();
        }
    }
}
