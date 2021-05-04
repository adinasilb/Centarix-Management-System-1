using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class ResourceCategory
    {
        [Key]
        public int ResourceCategoryID { get; set; }
        public string ResourceCategoryDescription { get; set; }
        public bool IsMain { get; set; }
        public bool IsResourceType { get; set; }
        public IEnumerable<ResourceResourceCategory> ResourceResourceCategories { get; set; }
    }
}
