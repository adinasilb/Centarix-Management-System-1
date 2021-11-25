using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using PrototypeWithAuth.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.CRUD
{
    public class VendorContactsProc : ApplicationDbContextProc
    {
        public VendorContactsProc(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : base(context, userManager)
        {

        }

        public IQueryable<Models.VendorContact> Read()
        {
            return _context.VendorContacts.AsNoTracking().AsQueryable();
        }

        public VendorContact ReadOneByPK(int VendorContactID)
        {
            return _context.VendorContacts.Where(vc => vc.VendorContactID == VendorContactID).AsNoTracking().FirstOrDefault();
        }

        public IQueryable<VendorContactWithDeleteViewModel> ReadAsVendorContactWithDeleteByVendorID(int id)
        {
            return _context.VendorContacts
                .Where(vc => vc.VendorID == id)
                .Select(vc => new VendorContactWithDeleteViewModel()
                {
                    VendorContact = vc,
                    Delete = false
                })
                .AsNoTracking().AsQueryable();
        }
    }
}
