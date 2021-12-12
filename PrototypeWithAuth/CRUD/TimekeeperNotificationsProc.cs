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
    public class TimekeeperNotificationsProc : ApplicationDbContextProc<TimekeeperNotification>
    {
        public TimekeeperNotificationsProc(ApplicationDbContext context, bool FromBase = false) : base(context)
        {
            if (!FromBase)
            {
                base.InstantiateProcs();
            }
        }

        public async Task<StringWithBool> DeleteByEHIDAsync(int EHID)
        {
            StringWithBool ReturnVal = new StringWithBool();
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var notifications = Read(new List<Expression<Func<TimekeeperNotification, bool>>> { n => n.EmployeeHoursID == EHID });
                    Remove(notifications);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    ReturnVal.Bool = false;
                    ReturnVal.String = AppUtility.GetExceptionMessage(ex);
                }
            }
            return ReturnVal;
        }

        public override async Task<StringWithBool> RemoveWithSaveChangesAsync(TimekeeperNotification notification)
        {
            StringWithBool ReturnVal = new StringWithBool();
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    Remove(notification);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    ReturnVal.SetStringAndBool(true, null);
                }
                catch (Exception ex)
                {
                    ReturnVal.Bool = false;
                    ReturnVal.String = AppUtility.GetExceptionMessage(ex);
                }
            }
            return ReturnVal;
        }

    }
}
