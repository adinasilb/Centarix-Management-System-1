using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class UserListRoles
    {
        public List<String> RolesToAdd { get; set; }
        public List<String> RolesToRemove { get; set; }
    }
}
