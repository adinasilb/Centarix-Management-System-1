using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
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
    public class RequestsProc : ApplicationDbContextProc<Request>
    {
        public RequestsProc(ApplicationDbContext context, bool FromBase = false) : base(context)
        {
            if (!FromBase)
            {
                base.InstantiateProcs();
            }
        }

        public override IQueryable<Request> ReadWithIgnoreQueryFilters(List<Expression<Func<Request, bool>>> wheres = null, List<ComplexIncludes<Request, ModelBase>> includes = null)
        {
            wheres.Add(r => !r.IsDeleted);
            return base.ReadWithIgnoreQueryFilters(wheres, includes);
        }


        public override IQueryable<Request> ReadOneWithIgnoreQueryFilters(List<Expression<Func<Request, bool>>> wheres = null, List<ComplexIncludes<Request, ModelBase>> includes = null)
        {
            wheres.Add(r => !r.IsDeleted);
            return base.ReadOneWithIgnoreQueryFilters(wheres, includes);
        }

        public override async Task<Request> ReadOneWithIgnoreQueryFiltersAsync(List<Expression<Func<Request, bool>>> wheres = null, List<ComplexIncludes<Request, ModelBase>> includes = null)
        {
            wheres.Add(r => !r.IsDeleted);
            return await base.ReadOneWithIgnoreQueryFiltersAsync(wheres, includes);
        }

        public async Task<StringWithBool> RequestPriceQuoteProcAsync(Request request)
        {
            StringWithBool ReturnVal = new StringWithBool();
            try
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        request.RequestStatusID = 6;
                        request.Cost = 0;
                        //newRequest.ParentQuote = new ParentQuote();
                        request.QuoteStatusID = 1;
                        request.OrderType = AppUtility.OrderTypeEnum.RequestPriceQuote.ToString();
                        //this is assuming that we only reorder request price quotes
                        _context.Entry(request).State = EntityState.Added;
                        //_context.Entry(newRequest.ParentQuote).State = EntityState.Added;
                        _context.Entry(request.Product).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                    }
                    catch (DbUpdateException ex)
                    {
                        await transaction.RollbackAsync();
                        throw ex;
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        throw ex;
                    }


                }
            }
            catch (Exception ex)
            {
                ReturnVal.SetStringAndBool(false, AppUtility.GetExceptionMessage(ex));
            }
            return ReturnVal;
        }


        public async Task<StringWithBool> SaveProprietaryAsync(Request newRequest, RequestItemViewModel requestItemViewModel, Guid guid, ReceivedModalVisualViewModel receivedModalVisualViewModel, string currentUserID)
        {
            StringWithBool ReturnVal = new StringWithBool();
            try
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        newRequest.RequestStatusID = 7;
                        newRequest.OrderType = AppUtility.OrderTypeEnum.Save.ToString();
                        newRequest.Unit = 1;
                        newRequest.Product.UnitTypeID = 5;
                        newRequest.Product.SerialNumber = await GetSerialNumberAsync(false);
                        if (newRequest.CreationDate == DateTime.Today) //if it's today, add seconds to be now so it shows up on top
                        {
                            newRequest.CreationDate = DateTime.Now;
                        }
                        _context.Add(newRequest);
                        await _context.SaveChangesAsync();

                        //await _context.SaveChangesAsync(); //what is this for?
                        if (receivedModalVisualViewModel.LocationInstancePlaces != null)
                        {
                            await _requestLocationInstancesProc.SaveLocationsAsync(receivedModalVisualViewModel, newRequest, false);
                        }
                        await _requestCommentsProc.UpdateAsync((List<RequestComment>)requestItemViewModel.Comments.Where(c => c.CommentTypeID==1), newRequest.RequestID, currentUserID);
                        await _productCommentsProc.UpdateAsync((List<ProductComment>)requestItemViewModel.Comments.Where(c => c.CommentTypeID==2), newRequest.ProductID, currentUserID);

                      
                        newRequest.Product = await _context.Products.Where(p => p.ProductID == newRequest.ProductID).FirstOrDefaultAsync();
                        RequestNotification requestNotification = new RequestNotification();
                        requestNotification.RequestID = newRequest.RequestID;
                        requestNotification.IsRead = false;
                        requestNotification.RequestName = newRequest.Product.ProductName;
                        requestNotification.ApplicationUserID = newRequest.ApplicationUserCreatorID;
                        requestNotification.Description = "item created";
                        requestNotification.NotificationStatusID = 2;
                        requestNotification.NotificationDate = DateTime.Now;
                        requestNotification.Controller = "Requests";
                        requestNotification.Action = "NotificationsView";
                        requestNotification.OrderDate = DateTime.Now;
                        await _requestNotificationsProc.CreateAsync(requestNotification);
                        await transaction.CommitAsync();
                    }
                    catch (DbUpdateException ex)
                    {
                        await transaction.RollbackAsync();
                        throw ex;
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal.SetStringAndBool(false, AppUtility.GetExceptionMessage(ex));
            }
            return ReturnVal;

        }

        public async Task<StringWithBool> ArchiveRequestAsync(Request request)
        {
            StringWithBool ReturnVal = new StringWithBool();
            try
            {
                request.IsArchived = true;
                _context.Update(request);
                await _context.SaveChangesAsync();
                ReturnVal.SetStringAndBool(true, null);
            }
            catch (Exception ex)
            {
                ReturnVal.SetStringAndBool(false, AppUtility.GetExceptionMessage(ex));
            }
            return ReturnVal;

        }
        public async Task<string> GetSerialNumberAsync(bool isOperations)
        {
            var categoryType = 1;
            var serialLetter = "L";
            int lastSerialNumberInt = 0;
            if (isOperations)
            {
                categoryType = 2;
                serialLetter = "P";
            }
            var serialnumberList = _productsProc.ReadWithIgnoreQueryFilters(new List<Expression<Func<Product, bool>>> { p => p.ProductSubcategory.ParentCategory.CategoryTypeID == categoryType })
                .Select(p => int.Parse(p.SerialNumber.Substring(1))).ToList();

            lastSerialNumberInt = serialnumberList.OrderBy(s => s).LastOrDefault();

            return serialLetter + (lastSerialNumberInt + 1);
        }



    }


}