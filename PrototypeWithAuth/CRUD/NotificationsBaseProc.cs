using Microsoft.AspNetCore.Identity;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.AppData;
using System.Linq.Expressions;

namespace PrototypeWithAuth.CRUD
{
    public class NotificationsBaseProc<T1, T2> : ApplicationDbContextProc<T1> where T1:Notification<T2> where T2:NotificationStatus
    {
        public NotificationsBaseProc(ApplicationDbContext context, bool FromBase = false) : base(context)
        {
            if (!FromBase)
            {
                base.InstantiateProcs();
            }
        }

        
        public virtual async Task<StringWithBool> CreateAsync(T1 notification)
        {
            StringWithBool ReturnVal = new StringWithBool();

            try
            {
                _context.Entry(notification).State = EntityState.Added;
                await _context.SaveChangesAsync();
                ReturnVal.SetStringAndBool(true, null);
            }
            catch (Exception ex)
            {
                ReturnVal.Bool = false;
                ReturnVal.String = AppUtility.GetExceptionMessage(ex);
            }
            return ReturnVal;
        }

        public virtual StringWithBool CreateWithoutSaveChanges(T1 notification)
        {
            StringWithBool ReturnVal = new StringWithBool();

            try
            {
                _context.Entry(notification).State = EntityState.Added;
                ReturnVal.SetStringAndBool(true, null);
            }
            catch (Exception ex)
            {
                ReturnVal.Bool = false;
                ReturnVal.String = AppUtility.GetExceptionMessage(ex);
            }
            return ReturnVal;
        }
    }
}
