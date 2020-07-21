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

        [Display(Name = "ID")]
        public string CentarixID { get; set; }
        public DateTime DateCreated { get; set; }
        public double LabMonthlyLimit { get; set; }
        public double LabUnitLimit { get; set; }
        public double LabOrderLimit { get; set; }
        public double OperationMonthlyLimit { get; set; }
        public double OperationUnitLimit { get; set; }
        public double OperaitonOrderLimit { get; set; }
        public bool IsDeleted { get; set; }
        public string UserImage { get; set; }

        // public string URLPic { get; set; }

    }
}
