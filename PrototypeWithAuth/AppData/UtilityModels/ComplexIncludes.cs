using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PrototypeWithAuth.AppData.UtilityModels
{
    public class ComplexIncludes<T>
    {
        public Expression<Func<T, object>> Include { get; set; }
        public ComplexIncludes<T> ThenInclude { get; set; }

        public IQueryable<T> RecursiveInclude(IQueryable<T> ObjectQueryable, ComplexIncludes<T> Include)
        {
            ObjectQueryable.ThenInclude(Include);
            if (Include.ThenInclude != null)
            {
                RecursiveInclude(ObjectQueryable, ThenInclude);
            }
            return ObjectQueryable;
        }
    }

}
