using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class CategoryBase : ModelBase
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
