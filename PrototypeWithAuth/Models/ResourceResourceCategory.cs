using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class ResourceResourceCategory
    {
        public int ResourceID { get; set; }
        public Resource Resource { get; set; }
        public int ResourceCategoryID { get; set; }
        public ResourceCategory ResourceCategory { get; set; }
    }
}
