using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.CRUD
{
    public class RequestNotificationsProc : NotificationsBaseProc<RequestNotification, RequestNotificationStatus>
    {
        public RequestNotificationsProc(ApplicationDbContext context, bool FromBase = false) : base(context, FromBase)
        {
        }


    }
}
