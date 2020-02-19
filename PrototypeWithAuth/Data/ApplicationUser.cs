using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using PrototypeWithAuth.Models;

namespace PrototypeWithAuth.Data
{
    public class ApplicationUser : IdentityUser // in order to customize the users
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IEnumerable<Request> Requests { get; set;}


        // public string URLPic { get; set; }

    }
}
