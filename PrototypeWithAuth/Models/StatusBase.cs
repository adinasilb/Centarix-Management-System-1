using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public abstract class StatusBase : ModelBase
    {
        [Key]
        public int ID { get; set; }
        public string Description { get; set; }
        public string DescriptionEnum { get; set; }
    }
}
