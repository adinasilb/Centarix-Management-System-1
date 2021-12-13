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
using System.Linq.Expressions;
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

        public async Task<StringWithBool> UpdateAsync(List<VendorContactWithDeleteViewModel> vendorContacts, int vendorID)
        {
            StringWithBool ReturnVal = new StringWithBool();
            try
            {
                foreach (var vendorContact in vendorContacts)
                {
                    if (vendorContact.Delete && vendorContact.VendorContact.VendorContactID != 0)
                    {
                        var dvc = await _vendorContactsProc.ReadOneAsync(new List<Expression<Func<VendorContact, bool>>> { vc => vc.VendorContactID == vendorContact.VendorContact.VendorContactID });
                        _context.Remove(dvc);
                    }
                    else if (!vendorContact.Delete)
                    {
                        vendorContact.VendorContact.VendorID = vendorID;
                        _context.Update(vendorContact.VendorContact);
                    }
                }
                await _context.SaveChangesAsync();
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
