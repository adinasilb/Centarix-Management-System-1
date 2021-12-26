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
    public class PaymentsProc : ApplicationDbContextProc<Payment>
    {
        public PaymentsProc(ApplicationDbContext context, bool FromBase = false) : base(context)
        {
            if (!FromBase) { this.InstantiateProcs(); }
        }

        public StringWithBool CreateWithoutSaveChanges(Payment payment)
        {
            StringWithBool ReturnVal = new StringWithBool();
            try
            {
                _context.Entry(payment).State = EntityState.Added;
                ReturnVal.SetStringAndBool(true, null);
            }
            catch (Exception ex)
            {
                ReturnVal.SetStringAndBool(false, AppUtility.GetExceptionMessage(ex));
            }
            return ReturnVal;

        }

        public async Task<StringWithBool> DeleteAsync(int requestID)
        {
            StringWithBool ReturnVal = new StringWithBool();
            try
            {
                var payments = Read(new List<Expression<Func<Payment, bool>>> { p => p.RequestID == requestID }).ToList();
                foreach (var payment in payments)
                {
                    payment.IsDeleted = true;
                    _context.Update(payment);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                ReturnVal.SetStringAndBool(false, AppUtility.GetExceptionMessage(ex));
            }
            return ReturnVal;           
        }

    }
}
