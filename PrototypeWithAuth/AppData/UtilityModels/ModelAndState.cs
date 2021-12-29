using Microsoft.EntityFrameworkCore;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.AppData.UtilityModels
{
    public class ModelAndState
    {
        public ModelBase Model { get; set; }
        public EntityState StateEnum{ get; set; }
    }
}
