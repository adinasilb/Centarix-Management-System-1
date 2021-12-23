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
        public RequestLocationInstancesProc(ApplicationDbContext context, bool FromBase = false) : base(context)
        {
            if (!FromBase) { this.InstantiateProcs(); }
        }


        public async Task<StringWithBool> SaveLocationsAsync(ReceivedModalVisualViewModel receivedModalVisualViewModel, Request requestReceived, bool archiveOneRequest)
        {

            StringWithBool ReturnVal = new StringWithBool();
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
                            _context.Add(rli);
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
                                await _locationInstancesProc.MarkLocationAvailableAsync(requestReceived.RequestID, place.LocationInstanceId);
                                return ReturnVal;
                            }
                            await _context.SaveChangesAsync();
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }

                        await _locationInstancesProc.MarkLocationInstanceAsFullAsync(locationInstance);
                    }
                }
            }
            catch (Exception ex)
            {
                ReturnVal.SetStringAndBool(false, AppUtility.GetExceptionMessage(ex));
            }
            return ReturnVal;


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
                            //archive one location and delete the rest
                            var iterator = requestLocations.GetEnumerator();
                            while (iterator.MoveNext())
                            {
                                var locationToDelete = iterator.Current;
                                _context.Remove(locationToDelete);
                                await _locationInstancesProc.MarkLocationAvailableAsync(requestId, locationToDelete.LocationInstanceID);
                            }
                            await _context.SaveChangesAsync();
                            await SaveLocationsAsync(receivedModalVisualViewModel, request, true);
                            await transaction.CommitAsync();
                        }
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        throw new Exception(AppUtility.GetExceptionMessage(ex));
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
