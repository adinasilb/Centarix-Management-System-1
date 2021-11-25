using Microsoft.AspNetCore.Identity;
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
        public OffDayTypesProc(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : base (context, userManager)
        {

        }

        public IQueryable<OffDayType> ReadManyByPKS(List<int> PKs)
        {
            return _context.OffDayTypes.Where(od => PKs.Contains(od.OffDayTypeID));
        }

        public OffDayType ReadOneByOffDayTypeEnum(AppUtility.OffDayTypeEnum OffDayTypeEnum)
        {
            return _context.OffDayTypes.Where(odt => odt.Description == AppUtility.GetDisplayNameOfEnumValue(OffDayTypeEnum.ToString())).FirstOrDefault();
        }

        public OffDayType ReadOneByPK(int ID)
        {
            return _context.OffDayTypes.Where(odt => odt.OffDayTypeID == ID).FirstOrDefault();
        }
    }
}
