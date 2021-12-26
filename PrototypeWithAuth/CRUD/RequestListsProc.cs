using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using PrototypeWithAuth.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PrototypeWithAuth.CRUD
{
    public class RequestListsProc : ApplicationDbContextProc<RequestList>
    {
        public RequestListsProc(ApplicationDbContext context, bool FromBase = false) : base(context)
        {
            if (!FromBase) { this.InstantiateProcs(); }
        }

        public async Task<RequestList> CreateAndGetDefaultListAsync(string userID)
        {

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    RequestList requestList = new RequestList
                    {
                        Title = "List 1",
                        ApplicationUserOwnerID = userID,
                        RequestListRequests = new ListImplementsModelBase<RequestListRequest>(),
                        DateCreated = DateTime.Now,
                        IsDefault = true
                    };

                    _context.Entry(requestList).State = EntityState.Added;
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return requestList;
                }
                catch (Exception ex)
                {
                    return null;
                }
                
            }
        }

        public async Task<RequestList> CreateAndGetAsync(NewListViewModel newListViewModel)
        {
            var newList = new RequestList();
            StringWithBool ReturnVal = new StringWithBool();
            try
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        newList.ApplicationUserOwnerID = newListViewModel.OwnerID;
                        newList.Title = newListViewModel.ListTitle;
                        newList.DateCreated = DateTime.Now;
                        _context.Entry(newList).State = EntityState.Added;

                        await _context.SaveChangesAsync();

                        if (newListViewModel.RequestToAddID != 0)
                        {
                            await _requestListRequestsProc.MoveListWithoutSaveChanges(newListViewModel.RequestToAddID, newList.ListID, newListViewModel.RequestPreviousListID);

                            await _context.SaveChangesAsync();
                        }
                        await transaction.CommitAsync();
                          ReturnVal.SetStringAndBool(true, null);
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        throw new Exception(AppUtility.GetExceptionMessage(ex));
                    }
                    ReturnVal.SetStringAndBool(true, null);
                }

            }
            catch (Exception ex)
            {
                ReturnVal.SetStringAndBool(false, AppUtility.GetExceptionMessage(ex));
            }
            return newList;
        }


        public async Task<StringWithBool> UpdateAsync(ListSettingsViewModel listSettings, string userID)
        {
            StringWithBool ReturnVal = new StringWithBool();
            try
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var viewModelList = listSettings.SidebarType == AppUtility.SidebarEnum.MyLists ? listSettings.SelectedList : listSettings.SelectedSharedList.RequestList;
                        var list = await ReadOneAsync(new List<Expression<Func<RequestList, bool>>> { rl => rl.ListID == viewModelList.ListID });
                        list.Title = viewModelList.Title;
                        _context.Update(list);
                        await _shareRequestListsProc.UpdateWithoutSaveChangesAsync(listSettings, userID);
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        throw new Exception(AppUtility.GetExceptionMessage(ex));
                    }
                    ReturnVal.SetStringAndBool(true, null);
                }

            }
            catch (Exception ex)
            {
                ReturnVal.SetStringAndBool(false, AppUtility.GetExceptionMessage(ex));
            }
            return ReturnVal;
        }

  

        public async Task<StringWithBool> DeleteAsync(RequestList deleteList)
        {
            StringWithBool ReturnVal = new StringWithBool();
            try
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var list = await ReadOneAsync(new List<Expression<Func<RequestList, bool>>> { l => l.ListID == deleteList.ListID },
                            new List<ComplexIncludes<RequestList, ModelBase>> { new ComplexIncludes<RequestList, ModelBase> { Include = l => l.RequestListRequests } });
                        _requestListRequestsProc.DeleteByRequestListWithoutSaveChanges(list);
                        _shareRequestListsProc.DeleteByListIDWithoutSaveChanges(deleteList.ListID);
                        _context.Remove(list);
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        throw new Exception(AppUtility.GetExceptionMessage(ex));

                    }
                    ReturnVal.SetStringAndBool(true, null);
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
