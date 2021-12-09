using PrototypeWithAuth.AppData;
using PrototypeWithAuth.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public  class CommentBase : ModelBase
    {
        private DateTime _CommentTimeStamp;
        [Key]
        public int CommentID { get; set; }
        public string ApplicationUserID { get; set; } //this is the owner of the request - do we have every received request have its own reciever?

        [ForeignKey("ApplicationUserID")]
        public ApplicationUser ApplicationUser { get; set; }
        public string CommentText { get; set; }

        [DataType(DataType.Date)]
        public DateTime CommentTimeStamp
        {
            get => _CommentTimeStamp==new DateTime() ? DateTime.Now : _CommentTimeStamp;
            set { _CommentTimeStamp = value; }
        }

        public int CommentTypeID { get; set; }
        public CommentType CommentType { get; set; }
        public bool IsDeleted { get; set; }
        public int ObjectID { get; set; }
    }
}
