using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.CRUD
{
    public class CategoryBasesProc<T1> : ApplicationDbContextProc<T1> where T1:CategoryBase
    {
        public CategoryBasesProc(ApplicationDbContext context, bool FromBase = false): base(context)
        {
            if (!FromBase)
            {
                base.InstantiateProcs();
            }
        }

    }
}
