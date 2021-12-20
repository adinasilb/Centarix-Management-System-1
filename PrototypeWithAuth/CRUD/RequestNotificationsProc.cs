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
    public class RequestNotificationsProc : ApplicationDbContextProc<RequestNotification>
    {
        public RequestNotificationsProc(ApplicationDbContext context, bool FromBase = false) : base(context)
        {
            if (!FromBase) { this.InstantiateProcs(); }
        }


        public StringWithBool CreateWithoutSaveChanges(RequestNotification notification)
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
