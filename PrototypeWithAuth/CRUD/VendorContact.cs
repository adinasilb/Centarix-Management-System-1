using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.CRUD
{
    public class VendorContact : ApplicationDbContextProcedure
    {
        public VendorContact(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : base(context, userManager)
        {

        }

        public IQueryable<Models.VendorContact> Read()
        {
            return _context.VendorContacts.AsNoTracking().AsQueryable();
        }

        public IQueryable<VendorContactWithDeleteViewModel> ReadAsVendorContactWithDeleteByVendorID(int id)
        {
            return _context.VendorContacts
                .Select(vc => new VendorContactWithDeleteViewModel()
                {
                    VendorContact = vc,
                    Delete = false
                })
                .AsNoTracking().AsQueryable();
        }
    }
}
