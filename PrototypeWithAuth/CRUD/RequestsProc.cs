using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PrototypeWithAuth.CRUD
{
    public class RequestsProc : ApplicationDbContextProc<Request>
    {
        public RequestsProc(ApplicationDbContext context, bool FromBase = false) : base(context)
        {
            if (!FromBase)
            {
                base.InstantiateProcs();
            }
        }

        public override IQueryable<Request> ReadWithIgnoreQueryFilters(List<Expression<Func<Request, bool>>> wheres = null, List<ComplexIncludes<Request, ModelBase>> includes = null)
        {
            wheres.Add(r => !r.IsDeleted);
            return base.ReadWithIgnoreQueryFilters(wheres, includes);
        }

        public override IQueryable<Request> ReadOneWithIgnoreQueryFilters(List<Expression<Func<Request, bool>>> wheres = null, List<ComplexIncludes<Request, ModelBase>> includes = null)
        {
            wheres.Add(r => !r.IsDeleted);
            return base.ReadOneWithIgnoreQueryFilters(wheres, includes);
        }

        public override async Task<Request> ReadOneWithIgnoreQueryFiltersAsync(List<Expression<Func<Request, bool>>> wheres = null, List<ComplexIncludes<Request, ModelBase>> includes = null)
        {
            wheres.Add(r => !r.IsDeleted);
            return await base.ReadOneWithIgnoreQueryFiltersAsync(wheres, includes);
        }
    }

}
