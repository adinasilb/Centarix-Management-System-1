using Microsoft.AspNetCore.Identity;
using PrototypeWithAuth.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.CRUD
{
    public class ApplicationUsersProc : ApplicationDbContextProcedure
    {
        public ApplicationUsersProc (ApplicationDbContext context, UserManager<ApplicationUser> userManager) : base (context, userManager)
        {

        }
    }
}
