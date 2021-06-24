using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using PrototypeWithAuth.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Index = Microsoft.EntityFrameworkCore.Metadata.Internal.Index;

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

        //[Display(Name = "ID")]
        //public string CentarixID { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; }
        public decimal LabMonthlyLimit { get; set; }
        public decimal LabUnitLimit { get; set; }
        public decimal LabOrderLimit { get; set; }
        public decimal OperationMonthlyLimit { get; set; }
        public decimal OperationUnitLimit { get; set; }
        public decimal OperationOrderLimit { get; set; }
        public bool IsSuspended { get; set; }
        public bool IsDeleted { get; set; }
        public string UserImage { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number 2")]
        public string PhoneNumber2 { get; set; }
        public bool NeedsToResetPassword { get; set; }
        public bool RememberTwoFactor { get; set; }
        public DateTime DateLastReadNotifications { get; set; }
        public DateTime LastLogin { get; set; }
        public IEnumerable<Request> RequestsReceived { get; set; }
        public IEnumerable<Request> RequestsCreated { get; set; }
        public IEnumerable<Resource> ResourcesCreated { get; set; }
        public IEnumerable<ShareRequest> ShareRequestsCreated { get; set; }
        public IEnumerable<ShareRequest> ShareRequestsReceived { get; set; }
        public IEnumerable<ShareResource> ShareResourcesCreated { get; set; }
        public IEnumerable<ShareResource> ShareResourcesReceived { get; set; }
        public IEnumerable<FavoriteRequest> FavoriteRequests { get; set; }
        public IEnumerable<FavoriteProtocol> FavoriteProtocols { get; set; }
        public IEnumerable<FavoriteResource> FavoriteResources { get; set; }

        // public string URLPic { get; set; }

    }
}
