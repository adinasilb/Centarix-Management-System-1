using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PrototypeWithAuth.AppData.Exceptions;
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


        public async Task SaveLocationsAsync(ReceivedModalVisualViewModel receivedModalVisualViewModel, Request requestReceived, bool archiveOneRequest)
        {
            //getting the parentlocationinstanceid
            var liParent = await _locationInstancesProc.ReadOneAsync(new List<Expression<Func<LocationInstance, bool>>> { li => li.LocationInstanceID == receivedModalVisualViewModel.ParentLocationInstance.LocationInstanceID });
            var mayHaveParent = true;
            while (mayHaveParent)
            {
                if (liParent.LocationInstanceParentID != null)
                {
                    liParent = await _locationInstancesProc.ReadOneAsync( new List<Expression<Func<LocationInstance, bool>>> { li => li.LocationInstanceID == liParent.LocationInstanceParentID });
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
                            return;
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


    }
}
