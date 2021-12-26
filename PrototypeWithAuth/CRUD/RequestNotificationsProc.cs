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

        public async Task<StringWithBool> CreateManyOrderLateWithoutSaveAsync(List<Request> orderLateRequests)
        {
            var ReturnVal = new StringWithBool() { Bool = true };
            try
            {
                foreach (var request in orderLateRequests)
                {
                    RequestNotification requestNotification = new RequestNotification();
                    requestNotification.RequestID = request.RequestID;
                    requestNotification.IsRead = false;
                    requestNotification.RequestName = request.Product.ProductName;
                    requestNotification.ApplicationUserID = request.ApplicationUserCreatorID;
                    requestNotification.Description = "should have arrived " + request.ParentRequest.OrderDate.AddDays(request.ExpectedSupplyDays ?? 0).GetElixirDateFormat();
                    requestNotification.NotificationStatusID = 1;
                    requestNotification.NotificationDate = DateTime.Now;
                    requestNotification.Controller = "Requests";
                    requestNotification.Action = "NotificationsView";
                    requestNotification.OrderDate = request.ParentRequest.OrderDate;
                    requestNotification.Vendor = request.Product.Vendor.VendorEnName;
                    ReturnVal = CreateWithoutSaveChanges(requestNotification);
                    if (!ReturnVal.Bool)
                    {
                        return ReturnVal;
                    }
                }
                ReturnVal = await SaveDbChangesAsync();
            }
            catch (Exception ex)
            {
                ReturnVal.SetStringAndBool(false, AppUtility.GetExceptionMessage(ex));
            }

            return ReturnVal;
        }
    }
}
