using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PrototypeWithAuth.CRUD
{
    public class InvoicesProc : ApplicationDbContextProc<Invoice>
    {
        public InvoicesProc(ApplicationDbContext context, bool FromBase = false) : base(context)
        {
            if (!FromBase) { this.InstantiateProcs(); }
            else
            {
                _paymentsProc = new PaymentsProc(context, true);
            }
        }
        public async Task<int> CreateAsync(AddInvoiceViewModel addInvoiceViewModel)
        {
            var validInvoiceNum = CheckUniqueVendorAndInvoiceNumber(addInvoiceViewModel.Vendor.VendorID, addInvoiceViewModel.Invoice.InvoiceNumber);
            if (!validInvoiceNum)
            {
                throw new Exception(ElixirStrings.ServerExistingInvoiceNumberVendorErrorMessage);
            }
            _context.Entry(addInvoiceViewModel.Invoice).State = EntityState.Added;
            //_context.Entry(addInvoiceViewModel.Invoice.Requests).State = EntityState.Detached;
            //_context.Update(addInvoiceViewModel.Invoice);
            await _context.SaveChangesAsync();
            return addInvoiceViewModel.Invoice.InvoiceID;
        }

        public async Task<StringWithBool> CreateAsyncWithTransaction(AddInvoiceViewModel addInvoiceViewModel)
        {
            StringWithBool ReturnVal = new StringWithBool();
            try
            {
                await this.CreateAsync(addInvoiceViewModel);
                ReturnVal.SetStringAndBool(true, null);
            }
            catch (Exception ex)
            {
                ReturnVal.SetStringAndBool(false, AppUtility.GetExceptionMessage(ex));
            }
            return ReturnVal;
        }

        public bool CheckUniqueVendorAndInvoiceNumber(int VendorID, string InvoiceNumber)
        {
            var boolCheck = true;
            var existingVendorInvoiceNumbers =  _paymentsProc.Read(new List<Expression<Func<Payment, bool>>> { p => p.Request.Product.VendorID == VendorID && p.Invoice != null }).Select(p => p.Invoice.InvoiceNumber).ToList();
            if (existingVendorInvoiceNumbers.Contains(InvoiceNumber))
            {
                boolCheck = false;
            }

            return boolCheck;
        }
        public async Task<bool> CheckUniqueVendorAndInvoiceNumberAsync(int VendorID, string InvoiceNumber)
        {
            var boolCheck = true;
            var existingVendorInvoiceNumbers1 = _paymentsProc.Read(new List<Expression<Func<Payment, bool>>> { p => p.Request.Product.VendorID == VendorID && p.Invoice != null });
            var existingVendorInvoiceNumbers = await existingVendorInvoiceNumbers1.Select(p => p.Invoice.InvoiceNumber).ToListAsync();
            if (existingVendorInvoiceNumbers.Contains(InvoiceNumber))
            {
                boolCheck = false;
            }

            return boolCheck;
        }


    }
}
