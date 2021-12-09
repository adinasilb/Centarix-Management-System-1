using Microsoft.AspNetCore.Identity;
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
    public class OffDayTypesProc : ApplicationDbContextProc<OffDayType>
    {
        public OffDayTypesProc(ApplicationDbContext context, bool FromBase = false) : base (context)
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

        public async Task<OffDayType> ReadOneByOffDayTypeEnumAsync(AppUtility.OffDayTypeEnum OffDayTypeEnum)
        {
            return await _context.OffDayTypes.Where(odt => odt.Description == AppUtility.GetDisplayNameOfEnumValue(OffDayTypeEnum.ToString()))
                .AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<OffDayType> ReadOneByPKAsync(int ID)
        {
            return await _context.OffDayTypes.Where(odt => odt.OffDayTypeID == ID).AsNoTracking().FirstOrDefaultAsync();
        }
    }
}
