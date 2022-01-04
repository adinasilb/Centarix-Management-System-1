using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.CRUD
{
    public class CompanyDaysOffProc : ApplicationDbContextProc<CompanyDayOff>
    {
        public CompanyDaysOffProc(ApplicationDbContext context, bool FromBase = false) : base (context)
        {
            if (!FromBase) { base.InstantiateProcs(); }
        }

    }
}
