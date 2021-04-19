using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class MaterialCategory
    {
        [Key]
        public int MaterialCategoryID { get; set; }
        public string MaterialCategoryDescription { get; set; }
    }
}
