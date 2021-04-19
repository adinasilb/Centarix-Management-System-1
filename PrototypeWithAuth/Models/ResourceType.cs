using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PrototypeWithAuth.Models
{
    public class ResourceType
    {
        [Key]
        public int ResourceTypeId { get; set; }
        public int ResourceTypeDescription { get; set; }
    }
}
