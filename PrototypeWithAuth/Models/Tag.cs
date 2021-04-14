using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PrototypeWithAuth.Models
{
    public class Tag
    {
        [Key]
        public int TagID { get; set; }
        public string TagDescription { get; set; }
    }
}
