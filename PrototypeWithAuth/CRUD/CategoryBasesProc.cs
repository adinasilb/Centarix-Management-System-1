﻿using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.CRUD
{
    public class CategoryBasesProc : ApplicationDbContextProc<CategoryBase>
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