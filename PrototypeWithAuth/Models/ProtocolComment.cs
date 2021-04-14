using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using PrototypeWithAuth.Data;

namespace PrototypeWithAuth.Models
{
    public class ProtocolComment
    {
        [Key]
        public int ProtocolCommentID { get; set; }
        public string ProtocolCommmentType { get; set; }
        public string ProtocolCommentDescription { get; set; }
        public string ApplicationUserCreatorID { get; set; }
        public ApplicationUser ApplicationUserCreator { get; set; }


        private DateTime _CommentTimeStamp;
        [DataType(DataType.Date)]
        public DateTime CommentTimeStamp
        {
            get => _CommentTimeStamp == new DateTime() ? DateTime.Now : _CommentTimeStamp;
            set { _CommentTimeStamp = value; }
        }
    }
}
