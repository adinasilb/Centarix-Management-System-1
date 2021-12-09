using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.AppData.UtilityModels
{
    public class ComplexIncludes<T>
    {
        public T Include { get; set; }
        public ComplexIncludes<T> ThenInclude { get; set; }
    }

}
