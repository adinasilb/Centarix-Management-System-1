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
    public class TimekeeperNotificationsProc : NotificationsBaseProc<TimekeeperNotification, TimekeeperNotificationStatus>
    {
        public TimekeeperNotificationsProc(ApplicationDbContext context, bool FromBase = false) : base(context, FromBase)
        {

        }

        public async Task<StringWithBool> DeleteByEHIDAsync(int EHID)
        {
            StringWithBool ReturnVal = new StringWithBool();
            try
            {
                var notifications = await Read(new List<Expression<Func<TimekeeperNotification, bool>>> { n => n.EmployeeHoursID == EHID }).ToListAsync();
                await DeleteAsync(notifications);
            }
            catch (Exception ex)
            {
                ReturnVal.Bool = false;
                ReturnVal.String = AppUtility.GetExceptionMessage(ex);
            }
            return ReturnVal;
        }

        public async Task<StringWithBool> DeleteByPKAsync(int id)
        {
            StringWithBool ReturnVal = new StringWithBool();
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var notification = await _timekeeperNotificationsProc.ReadOneAsync(new List<Expression<Func<TimekeeperNotification, bool>>> { tn => tn.NotificationID == id });
                    _context.Remove(notification);
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

        public override Task<StringWithBool> CreateWithoutTransactionAsync(TimekeeperNotification notification)
        {
            return base.CreateWithoutTransactionAsync(notification);
        }

    }
}
