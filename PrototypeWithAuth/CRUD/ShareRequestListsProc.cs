using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.ViewModels;

namespace PrototypeWithAuth.CRUD
{
    public class ShareRequestListsProc : ApplicationDbContextProc<ShareRequestList>
    {
        public ShareRequestListsProc(ApplicationDbContext context, bool FromBase = false) : base(context)
        {
            if (!FromBase)
            {
                base.InstantiateProcs();
            }
        }

        public StringWithBool DeleteByListIDWithoutSaveChanges(int id)
        {
            StringWithBool ReturnVal = new StringWithBool();
            try
            {
                var shareRequestLists = _shareRequestListsProc.Read(new List<Expression<Func<ShareRequestList, bool>>> { srl => srl.RequestListID == id }).ToList();
                foreach (var srl in shareRequestLists)
                {
                    _context.Remove(srl);
                }
            }
            catch (Exception ex)
            {
                ReturnVal.SetStringAndBool(false, AppUtility.GetExceptionMessage(ex));
            }
            return ReturnVal;
         
        }


        public async Task<StringWithBool> UpdateWithoutSaveChangesAsync(ListSettingsViewModel listSettings, string userID)
        {
            StringWithBool ReturnVal = new StringWithBool();
            try
            {
                if (listSettings.SharedUsers != null)
                {
                    foreach (var user in listSettings.SharedUsers)
                    {
                        if (!user.IsRemoved)
                        {
                            if (user.ShareRequestList.ShareID == 0)
                            {
                                user.ShareRequestList.ToApplicationUser = null;
                                user.ShareRequestList.RequestListID = listSettings.SelectedList.ListID;
                                user.ShareRequestList.FromApplicationUserID = userID;
                                _context.Add(user.ShareRequestList);
                            }
                            else
                            {
                                var sharelist = await ReadOneAsync(new List<Expression<Func<ShareRequestList, bool>>> { srl => srl.ShareID == user.ShareRequestList.ShareID });
                                sharelist.ViewOnly = user.ShareRequestList.ViewOnly;
                                _context.Update(sharelist);
                            }
                        }
                        else
                        {
                            if (user.ShareRequestList.ShareID != 0)
                            {
                                _context.Remove(user.ShareRequestList);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal.SetStringAndBool(false, AppUtility.GetExceptionMessage(ex));
            }
            return ReturnVal;
            
        }
    }
}
