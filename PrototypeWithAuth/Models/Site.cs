using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class Site
    {
        [Key]
        public int SiteID { get; set; }
        public string Name { get; set; }
    }
}
