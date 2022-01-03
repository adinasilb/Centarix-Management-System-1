using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.AppData.Exceptions;
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
    public class RequestLocationInstancesProc : ApplicationDbContextProc<RequestLocationInstance>
    {
        public RequestLocationInstancesProc(ApplicationDbContext context, bool FromBase = false, bool flag = false) : base(context)
        {
            if (!FromBase) { this.InstantiateProcs(); }
            else if(!flag)
            {
                _locationInstancesProc = new LocationInstancesProc(context, true);
                _temporaryLocationInstancesProc = new TemporaryLocationInstancesProc(context, true);
            }
        }

        public async Task SaveTempLocationWithoutTransactionAsync(ReceivedLocationViewModel receivedLocationViewModel, Request requestReceived)
        {
            var tempLocationInstance = await _temporaryLocationInstancesProc.ReadOneAsync(new List<Expression<Func<TemporaryLocationInstance, bool>>> { tli => tli.LocationTypeID == receivedLocationViewModel.LocationTypeID });
            await _temporaryLocationInstancesProc.CreateWithoutTransactionAsync(tempLocationInstance, receivedLocationViewModel.LocationTypeID);
            var rli = new RequestLocationInstance()
            {
                LocationInstanceID = tempLocationInstance.LocationInstanceID,
                RequestID = requestReceived.RequestID,
            };
            _context.Add(rli);
            await _context.SaveChangesAsync();
        }
        public async Task SaveLocationsWithoutTransactionAsync(ReceivedModalVisualViewModel receivedModalVisualViewModel, Request requestReceived, bool archiveOneRequest)
        {
            try
            {
                //getting the parentlocationinstanceid
                var liParent = await _locationInstancesProc.ReadOneAsync(new List<Expression<Func<LocationInstance, bool>>> { li => li.LocationInstanceID == receivedModalVisualViewModel.ParentLocationInstance.LocationInstanceID });
                var mayHaveParent = true;
                while (mayHaveParent)
                {
                    if (liParent.LocationInstanceParentID != null)
                    {
                        liParent = await _locationInstancesProc.ReadOneAsync(new List<Expression<Func<LocationInstance, bool>>> { li => li.LocationInstanceID == liParent.LocationInstanceParentID });
                    }
                    else
                    {
                        mayHaveParent = false;
                    }
                }
                foreach (var place in receivedModalVisualViewModel.LocationInstancePlaces)
                {
                    if (place.Placed)
                    {

                        //adding the requestlocationinstance
                        var rli = new RequestLocationInstance()
                        {
                            LocationInstanceID = place.LocationInstanceId,
                            RequestID = requestReceived.RequestID,
                            ParentLocationInstanceID = liParent.LocationInstanceID
                        };
                        var locationInstance = await _locationInstancesProc.ReadOneAsync(new List<Expression<Func<LocationInstance, bool>>> { li => li.LocationInstanceID == place.LocationInstanceId });
                        if (locationInstance.IsFull == false)
                        {
                            if(archiveOneRequest)
                            {
                                rli.IsArchived = true;
                            }
                            _context.Entry(rli).State = EntityState.Added;
                        }
                        else
                        {
                            throw new LocationAlreadyFullException();
                        }
                        try
                        {
                            if (archiveOneRequest)
                            {
                                await _requestsProc.ArchiveRequestAsync(requestReceived);
                                await _locationInstancesProc.MarkLocationAvailableWithoutSaveChangesAsync(requestReceived.RequestID, place.LocationInstanceId);
                            }
                            else
                            {
                               await _locationInstancesProc.MarkLocationInstanceAsFullAsync(locationInstance);
                            }
                            await _context.SaveChangesAsync();
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("RequestLocationInstancesProc SaveLocationsAsync in archive request catch-" + AppUtility.GetExceptionMessage(ex));
                        }

                           }
                }
            }
            catch (Exception ex)
            {
               throw new Exception("RequestLocationInstancesProc SaveLocationsAsync-" + AppUtility.GetExceptionMessage(ex));
            }

        }

        public async Task<StringWithBool> ArchiveAsync(int requestId, ReceivedModalVisualViewModel receivedModalVisualViewModel)
        {
            StringWithBool ReturnVal = new StringWithBool();
            try
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        if (receivedModalVisualViewModel.LocationInstancePlaces != null)
                        {
                            var request = await _requestsProc.ReadOneAsync( new List<Expression<Func<Request, bool>>> { r => r.RequestID == requestId }, new List<ComplexIncludes<Request, ModelBase>> { 
                            new ComplexIncludes<Request, ModelBase>{ Include = r=>r.RequestLocationInstances}
                            });
                            var requestLocations = request.RequestLocationInstances;
                            await DeleteWithoutTransactionAsync(requestLocations);
                            await SaveLocationsWithoutTransactionAsync(receivedModalVisualViewModel, request, true);
                            await transaction.CommitAsync();
                        }
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

        public async Task DeleteWithoutTransactionAsync(List<RequestLocationInstance> requestLocationInstances)
        {

            try
            {                
                foreach (var requestLocationInstance in requestLocationInstances)
                {

                    await _locationInstancesProc.MarkLocationAvailableWithoutSaveChangesAsync(requestLocationInstance.RequestID, requestLocationInstance.LocationInstanceID);
                    _context.Entry(requestLocationInstance).State = EntityState.Deleted;
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("RequestLocationInstanceProc DeleteAsync -"+ AppUtility.GetExceptionMessage(ex));
            }
         
        }

        public async Task<StringWithBool> UpdateAsync(ReceivedModalVisualViewModel receivedModalVisualViewModel, int requestID)
        {
            StringWithBool ReturnVal = new StringWithBool();
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await UpdateWithoutTransactionAsync(receivedModalVisualViewModel, requestID);
                    await transaction.CommitAsync();
                    ReturnVal.SetStringAndBool(true, null);
                }
                catch (Exception ex)
                {
                    ReturnVal.SetStringAndBool(false, AppUtility.GetExceptionMessage(ex));
                }
              
            }
            return ReturnVal;
        }

        public async Task UpdateWithoutTransactionAsync(ReceivedModalVisualViewModel receivedModalVisualViewModel, int requestID)
        {
            try
            {
                if (receivedModalVisualViewModel.LocationInstancePlaces != null)
                {
                    var request = await _requestsProc.ReadOneAsync(new List<Expression<Func<Request, bool>>> { r => r.RequestID == requestID }, new List<ComplexIncludes<Request, ModelBase>> {
                            new ComplexIncludes<Request, ModelBase>{ Include = r => r.RequestLocationInstances } });
                    var requestLocations = request.RequestLocationInstances;
                    await DeleteWithoutTransactionAsync(requestLocations);
                    receivedModalVisualViewModel.ParentLocationInstance = _context.LocationInstances.Where(li => li.LocationInstanceID == receivedModalVisualViewModel.ParentLocationInstance.LocationInstanceID).FirstOrDefault();

                    await SaveLocationsWithoutTransactionAsync(receivedModalVisualViewModel, request, false);
                }

            }
            catch (Exception ex)
            {
                throw new Exception( AppUtility.GetExceptionMessage(ex));
            }
        }
    } 
}
