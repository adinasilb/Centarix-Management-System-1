﻿using Microsoft.AspNetCore.Identity;
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

        public async Task<CompanyDayOff> ReadOneByDateAsync(DateTime date)
        {
            return await _context.CompanyDayOffs.Where(cdo => cdo.Date.Date == date).AsNoTracking().FirstOrDefaultAsync();
        }

        public IQueryable<CompanyDayOff> ReadByDateSpan(DateTime DateFrom, DateTime DateTo)
        {
            return _context.CompanyDayOffs.Where(d => d.Date >= DateFrom && d.Date <= DateTo).AsQueryable();
        }

        public IQueryable<CompanyDayOff> ReadByDate(DateTime date)
        {
            return _context.CompanyDayOffs.Where(cdo => cdo.Date.Date == date).AsQueryable();
        }

    }
}
