using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class CommentType
    {
        [Key]
        public int TypeID { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public string IconActionClass { get; set; }
        public string DescriptionEnum { get; set; }
        public string Color { get; set; }
    }
}
