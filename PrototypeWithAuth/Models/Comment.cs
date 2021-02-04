using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using PrototypeWithAuth.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrototypeWithAuth.Models
{
    public class Comment
    {
        private DateTime _CommentTimeStamp;
        [Key]
        public int CommentID { get; set; }
        public int RequestID { get; set; }
        public Request Request { get; set; }
        public string ApplicationUserID { get; set; } //this is the owner of the request - do we have every received request have its own reciever?

        [ForeignKey("ApplicationUserID")]
        public ApplicationUser ApplicationUser { get; set; }
        public string CommentText { get; set; }

        [DataType(DataType.Date)]
        public DateTime CommentTimeStamp
        {
            get => _CommentTimeStamp==new DateTime()? DateTime.Now : _CommentTimeStamp;
            set { _CommentTimeStamp = value; } 
        }

        public string CommentType { get; set; }
        public bool IsDeleted { get; set; }
    }
}

