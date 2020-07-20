using PrototypeWithAuth.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class VendorComment
    {
        [Key]
        public int VendorCommentID { get; set; }
        public string ApplicationUserID { get; set; } //this is the owner of the request - do we have every received request have its own reciever?

        public int VendorID { get; set; }
        public Vendor Vendor { get; set; }

        [ForeignKey("ApplicationUserID")]
        public ApplicationUser ApplicationUser { get; set; }
        [Required]
        public string CommentText { get; set; }
        public string CommentType { get; set; }

        [DataType(DataType.Date)]
        private DateTime commentTimeStamp = DateTime.Now; //should this be readonly -ADINA
        public DateTime CommentTimeStamp
        {
            get => commentTimeStamp;
            set { }
        }
    }
}
