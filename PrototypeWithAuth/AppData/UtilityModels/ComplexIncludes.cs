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
        public ComplexIncludes<object> ThenInclude { get; set; }
      
    }

}
