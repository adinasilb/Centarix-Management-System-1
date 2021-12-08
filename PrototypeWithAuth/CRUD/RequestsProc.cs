﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PrototypeWithAuth.CRUD
{
    public class RequestsProc : ApplicationDbContextProc
    {
        public RequestsProc(ApplicationDbContext context, UserManager<ApplicationUser> userManager, bool FromBase = false) : base(context, userManager)
        {
            if (!FromBase)
            {
                base.InstantiateProcs();
            }
        }

        public IQueryable<Request> ReadWithRequestsLocationInsances(List<Expression<Func<Request, bool>>> wheres = null, List<Expression<Func<Request, object>>> includes = null)
        {
            var requests = _context.Requests.Where(r => !r.IsDeleted)
                .Include(r => r.RequestLocationInstances).ThenInclude(li => li.LocationInstance)
                .ThenInclude(l => l.LocationInstanceParent)
                .AsNoTracking().AsQueryable();
            if (wheres != null)
            {
                foreach (var t in wheres)
                {
                    requests = requests.Where(t);
                }
            }
            if (includes != null)
            {
                foreach (var t in includes)
                {
                    requests = requests.Include(t);
                }
            }
            return requests;
        }
    }

}
