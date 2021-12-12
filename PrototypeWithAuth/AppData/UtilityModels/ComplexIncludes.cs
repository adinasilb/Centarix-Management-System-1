using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PrototypeWithAuth.Models;
using Serialize.Linq.Serializers;

namespace PrototypeWithAuth.AppData.UtilityModels
{
    public class ComplexIncludes<T, ModelBase> where T:ModelBase
    {
        public Expression<Func<T, ModelBase>> Include { get; set; }
        public ComplexIncludes<ModelBase, ModelBase>  ThenInclude { get; set; }

    }

}
