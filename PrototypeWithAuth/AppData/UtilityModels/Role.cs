using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.AppData.UtilityModels
{
    public class Role //Mimic an enum but be allowed to name them
    {
        public string RoleName { get; set; }
        public string RoleDefinition { get; set; }
        public bool IsMain { get; set; }
    }
}
