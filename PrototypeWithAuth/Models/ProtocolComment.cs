using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PrototypeWithAuth.Models
{
    public class ProtocolComment
    {
        [Key]
        public int ProtocolCommentID { get; set; }
        public string ProtocolCommmentType { get; set; }
        public string ProtocolCommentDescription { get; set; }
    }
}
