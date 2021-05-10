using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.AppData.UtilityModels
{
    public class StringWithName //Mimic an enum but be allowed to name them
    {
        public string StringName { get; set; }
        public string StringDefinition { get; set; }
    }
}
