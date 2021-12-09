﻿using Microsoft.AspNetCore.Identity;
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
    public class VendorCategoryTypesProc : ApplicationDbContextProc<VendorCategoryType>
    {
        public VendorCategoryTypesProc(ApplicationDbContext context, UserManager<ApplicationUser> userManager, bool FromBase = false) : base(context, userManager)
        {
            if (!FromBase)
            {
                base.InstantiateProcs();
            }
        }

        public async Task<StringWithBool> CreateWithoutSavingAsync(int VendorID, int CategoryTypeID)
        {
            StringWithBool ReturnVal = new StringWithBool();
            try
            {
                VendorCategoryType vct = new VendorCategoryType()
                {
                    CategoryTypeID = CategoryTypeID,
                    VendorID = VendorID
                };
                _context.Add(vct);
                ReturnVal.Bool = true;
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
