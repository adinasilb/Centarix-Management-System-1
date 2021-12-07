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

namespace PrototypeWithAuth.CRUD
{
    public class TimekeeperNotificationsProc : ApplicationDbContextProc
    {
        public TimekeeperNotificationsProc(ApplicationDbContext context, UserManager<ApplicationUser> userManager, bool FromBase = false) : base(context, userManager)
        {
            if (!FromBase)
            {
                base.InstantiateProcs();
            }
        }

        public async Task<StringWithBool> CreateAsync(TimekeeperNotification timekeeperNotification)
        {
            StringWithBool ReturnVal = new StringWithBool();
            try
            {
                _context.Add(timekeeperNotification);
                await _context.SaveChangesAsync();
                ReturnVal.SetStringAndBool(true, null);
            }
            catch (Exception ex)
            {
                ReturnVal.SetStringAndBool(false, AppUtility.GetExceptionMessage(ex));
            }
            return ReturnVal;
        }

        public IQueryable<TimekeeperNotification> ReadByUserID(string UserID)
        {
            return _context.TimekeeperNotifications.Where(n => n.ApplicationUserID == UserID)
                .Include(n => n.EmployeeHours).AsNoTracking().AsQueryable();
        }

        public IQueryable<TimekeeperNotification> ReadByEHID(int EHID)
        {
            return _context.TimekeeperNotifications.Where(n => n.EmployeeHoursID == EHID).AsNoTracking().AsQueryable();
        }

        public TimekeeperNotification ReadByPK(int? ID)
        {
            return _context.TimekeeperNotifications.Where(tn => tn.NotificationID == ID).FirstOrDefault();
        }

        public async Task<StringWithBool> DeleteByEHID(int EHID)
        {
            StringWithBool ReturnVal = new StringWithBool();
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var notifications = this.ReadByEHID(EHID);
                    this.DeleteWithoutSaving(notifications);
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

        public async Task<StringWithBool> DeleteByPK(int? ID)
        {
            StringWithBool ReturnVal = new StringWithBool();
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var notifications = new List<TimekeeperNotification>() { this.ReadByPK(ID) };
                    this.DeleteWithoutSaving(notifications);
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
