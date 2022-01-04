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
    public class EmployeeInfoNotificationsProc : NotificationsBaseProc<EmployeeInfoNotification, EmployeeInfoNotificationStatus>
    {
        public EmployeeInfoNotificationsProc(ApplicationDbContext context, bool FromBase = false) : base(context, FromBase)
        {
        }


    }
}
