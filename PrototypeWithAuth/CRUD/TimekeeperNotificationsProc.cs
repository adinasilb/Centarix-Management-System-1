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

      
        public IQueryable<TimekeeperNotification> ReadByUserID(string UserID, List<Expression<Func<TimekeeperNotification, object>>> includes = null)
        {
            var notifications = _context.TimekeeperNotifications.Where(n => n.ApplicationUserID == UserID);
            if (includes != null)
            {
                foreach (var t in includes)
                {
                    notifications = notifications.Include(t);
                }
            }
            return notifications.AsNoTracking().AsQueryable();
 
        }

        public IQueryable<TimekeeperNotification> ReadByEHID(int EHID)
        {
            return _context.TimekeeperNotifications.Where(n => n.EmployeeHoursID == EHID).AsNoTracking().AsQueryable();
        }

        public async Task<TimekeeperNotification> ReadByPKAsync(int? ID)
        {
            return await _context.TimekeeperNotifications.Where(tn => tn.NotificationID == ID).FirstOrDefaultAsync();
        }

        public async Task<StringWithBool> DeleteByEHIDAsync(int EHID)
        {
            StringWithBool ReturnVal = new StringWithBool();
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var notifications = this.ReadByEHID(EHID);
                    DeleteWithoutSaving(notifications);
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

        public async Task<StringWithBool> DeleteByPKAsync(int? ID)
        {
            StringWithBool ReturnVal = new StringWithBool();
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var notifications = new List<TimekeeperNotification>() {  await ReadByPKAsync(ID) };
                    DeleteWithoutSaving(notifications);
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

        public StringWithBool DeleteWithoutSaving(IEnumerable<TimekeeperNotification> notifications)
        {
            StringWithBool ReturnVal = new StringWithBool();
            try
            {
                foreach (TimekeeperNotification n in notifications)
                {
                    _context.Remove(n);
                }
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
