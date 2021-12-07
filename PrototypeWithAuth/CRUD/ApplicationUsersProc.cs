using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PrototypeWithAuth.CRUD
{
    public class ApplicationUsersProc : ApplicationDbContextProc
    {
        public ApplicationUsersProc (ApplicationDbContext context, UserManager<ApplicationUser> userManager, bool FromBase = false) : base (context, userManager)
        {
            if (!FromBase) { this.InstantiateProcs(); }
        }     

       
    }
}
