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
        private DateTime commentTimeStamp = DateTime.Now; //should this be readonly -ADINA
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
            get => commentTimeStamp;
            set { } 
        }

        public string CommentType { get; set; }
    }
}

