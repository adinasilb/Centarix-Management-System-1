using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.CRUD
{
    public class JobSubcategoryTypesProc : ApplicationDbContextProc<JobSubcategoryType>
    {
        public JobSubcategoryTypesProc(ApplicationDbContext context, bool FromBase = false) : base(context)
        {
            if (!FromBase) { base.InstantiateProcs(); }
        }
    }
}
