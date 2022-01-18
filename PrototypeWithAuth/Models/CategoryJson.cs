using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class CategoryJson : ModelBase
    {
        [Key]
        public int CategoryJsonID { get; set; }
        public int CategoryBaseID { get; set; }
        public CategoryBase Category { get; set; }
    }
}
