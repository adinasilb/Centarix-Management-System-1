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

        public async Task<StringWithBool> UpdateAsync(List<VendorContactWithDeleteViewModel> vendorContacts, int vendorID)
        {
            StringWithBool ReturnVal = new StringWithBool();
            try
            {
                foreach (var vendorContact in vendorContacts)
                {
                    if (vendorContact.Delete && vendorContact.VendorContact.VendorContactID != 0)
                    {
                        var dvc = await _vendorContactsProc.ReadOne(new List<Expression<Func<VendorContact, bool>>> { vc => vc.VendorContactID == vendorContact.VendorContact.VendorContactID });
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
