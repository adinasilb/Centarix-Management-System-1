using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using PrototypeWithAuth.Models;

namespace PrototypeWithAuth.Data
{
    public class ApplicationUser : IdentityUser // in order to customize the users
    {
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        public IEnumerable<ParentRequest> ParentRequests { get; set;}


        // public string URLPic { get; set; }

    }
}
