﻿using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.CRUD
{
    public class ShareRequestsProc : ApplicationDbContextProc<ShareRequest>
    {
        public ShareRequestsProc(ApplicationDbContext context, bool FromBase = false) : base(context)
        {
            if (!FromBase)
            {
                this.InstantiateProcs();
            }
        }
    }
}