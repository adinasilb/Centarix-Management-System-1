﻿using Microsoft.AspNetCore.Identity;
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
    public class ParentRequestsProc : ApplicationDbContextProc<ParentRequest>
    {
        public ParentRequestsProc(ApplicationDbContext context, bool FromBase = false) : base(context)
        {
            if (!FromBase) { this.InstantiateProcs(); }
        }


        public async Task<StringWithBool> DeleteAsync(ParentRequest parentRequest, int requestID)
        {

            StringWithBool ReturnVal = new StringWithBool();
            try
            {
                parentRequest.Requests = _context.Requests.Where(r => r.ParentRequestID == parentRequest.ParentRequestID && r.IsDeleted != true).ToList();
                if (parentRequest.Requests.Count() == 0)
                {
                    parentRequest.IsDeleted = true;
                    await _paymentsProc.DeleteAsync(requestID);
                    _context.Update(parentRequest);
                    await _context.SaveChangesAsync();
                }

                ReturnVal.SetStringAndBool(true, null);
            }
            catch (Exception ex)
            {
                ReturnVal.SetStringAndBool(false, AppUtility.GetExceptionMessage(ex));
            }
            return ReturnVal;

        }
    }
}
