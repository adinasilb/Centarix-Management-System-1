﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.CRUD
{
    public class OffDayTypesProc : ApplicationDbContextProc
    {
        public OffDayTypesProc(ApplicationDbContext context, UserManager<ApplicationUser> userManager, bool FromBase = false) : base (context, userManager)
        {
            if (!FromBase)
            {
                base.InstantiateProcs();
            }
        }

        public IQueryable<OffDayType> ReadManyByPKS(List<int> PKs)
        {
            return _context.OffDayTypes.Where(od => PKs.Contains(od.OffDayTypeID));
        }

        public OffDayType ReadOneByOffDayTypeEnum(AppUtility.OffDayTypeEnum OffDayTypeEnum)
        {
            return _context.OffDayTypes.Where(odt => odt.Description == AppUtility.GetDisplayNameOfEnumValue(OffDayTypeEnum.ToString()))
                .AsNoTracking().FirstOrDefault();
        }

        public OffDayType ReadOneByPK(int ID)
        {
            return _context.OffDayTypes.Where(odt => odt.OffDayTypeID == ID).AsNoTracking().FirstOrDefault();
        }
    }
}
