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
                        RequestListRequests = new List<RequestListRequest>(),
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


    }

}
