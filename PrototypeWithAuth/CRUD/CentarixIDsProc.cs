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
    public class CentarixIDsProc : ApplicationDbContextProc<CentarixID>
    {
        public CentarixIDsProc(ApplicationDbContext context, bool FromBase = false) : base(context)
        {
            if (!FromBase) { base.InstantiateProcs(); }
        }

        public void CreateWithoutSaving(CentarixID centarixID)
        {
            _context.Add(centarixID);
        }
    }
}
