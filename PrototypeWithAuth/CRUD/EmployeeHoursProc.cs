using Microsoft.AspNetCore.Identity;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.CRUD
{
    public class EmployeeHoursProc : ApplicationDbContextProcedure
    {
        public EmployeeHoursProc(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : base(context, userManager)
        {

        }

        public EmployeeHours ReadOneByDateAndUserID(DateTime dateTime, string UserID)
        {
            return _context.EmployeeHours.Where(eh => eh.Date.Date == dateTime.Date && eh.EmployeeID == UserID).FirstOrDefault();
        }

        public StringWithBool ReportHours()
        {
            StringWithBool ReturnVal = new StringWithBool();
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var userid = _userManager.GetUserId(User);
                    var todaysEntry = _context.EmployeeHours
                        .Include(eh => eh.OffDayType)
                        .Where(eh => eh.Date.Date == DateTime.Today.Date && eh.EmployeeID == userid).FirstOrDefault();
                    if (todaysEntry != null && todaysEntry.OffDayTypeID != null)
                    {
                        todaysEntry.OffDayTypeID = null;
                        //entryExitViewModel.OffDayRemoved = todaysEntry.OffDayType.Description;
                    }
                }
                catch (Exception ex)
                {
                }
            }
            return ReturnVal;
        }
    }
}
