using Microsoft.AspNetCore.Identity;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PrototypeWithAuth.CRUD
{
    public class TimekeeperNotificationsProc : ApplicationDbContextProcedure
    {
        public TimekeeperNotificationsProc(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : base(context, userManager)
        {

        }

        public IQueryable<TimekeeperNotification> ReadByUserID (string UserID)
        {
            return _context.TimekeeperNotifications.Where(n => n.ApplicationUserID == UserID)
                .Include(n => n.EmployeeHours).AsNoTracking().AsQueryable();
        }
    }
}
