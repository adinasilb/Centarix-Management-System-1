using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.AppData;

namespace PrototypeWithAuth.AppData.UtilityModels
{
    public class UserRoleList
    {
        public String RoleA { get; set; }
        public String RoleAEnumType { get; set; }
        public List<UserRoleList> RolesB { get; set; }
    }
}
