using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using PrototypeWithAuth.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.CRUD
{
    public class VendorContactsProc : ApplicationDbContextProc<VendorContact>
    {
        public VendorContactsProc(ApplicationDbContext context, bool FromBase = false) : base(context)
        {
            if (!FromBase)
            {
                base.InstantiateProcs();
            }
         }

        public IQueryable<Models.VendorContact> Read()
        {
            return _context.VendorContacts.AsNoTracking().AsQueryable();
        }

        public async Task<VendorContact> ReadOneByPKAsync(int VendorContactID)
        {
            return await _context.VendorContacts.Where(vc => vc.VendorContactID == VendorContactID).AsNoTracking().FirstOrDefaultAsync();
        }

        public IQueryable<VendorContactWithDeleteViewModel> ReadAsVendorContactWithDeleteByVendorIDAsync(int id)
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

        public StringWithBool Remove(VendorContact item)
        {
            StringWithBool ReturnVal = new StringWithBool();
            try
            {
                _context.Remove(item);
                ReturnVal.SetStringAndBool(true, null);
            }
            catch (Exception ex)
            {
                ReturnVal.SetStringAndBool(false, AppUtility.GetExceptionMessage(ex));
            }
            return ReturnVal;
        }
    }
}
