using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using PrototypeWithAuth.Models;
using System.ComponentModel.DataAnnotations;

namespace PrototypeWithAuth.Data
{
    public class ApplicationUser : IdentityUser // in order to customize the users
    {
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Google Secure App Password")]
        public string SecureAppPass { get; set; }
        public IEnumerable<ParentRequest> ParentRequests { get; set;}
        public int UserNum { get; set; }
        public DateTime DateCreated { get; set; }

        // public string URLPic { get; set; }

    }
}
