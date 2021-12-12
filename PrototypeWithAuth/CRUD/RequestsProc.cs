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

        public IQueryable<Request> ReadWithRequestsLocationInsances(List<Expression<Func<Request, bool>>> wheres = null, List<ComplexIncludes<Request, ModelBase>> includes = null)
        {
            var requests = _context.Requests.Where(r => !r.IsDeleted)
          .AsQueryable();
            if (wheres != null)
            {
                foreach (var t in wheres)
                {
                    requests = requests.Where(t);
                }
            }
            IIncludableQueryable<Request, ModelBase> requestsWithInclude = null;
            if (includes != null)
            {
                foreach (var t in includes)
                {
                    requestsWithInclude = requests.Include(t.Include);
                    if (t.ThenInclude !=null)
                    {
                        RecursiveInclude(requestsWithInclude, t.ThenInclude);
                    }

                }
            }
            return requestsWithInclude.AsNoTracking().AsQueryable();
        }
      
    }  

}
