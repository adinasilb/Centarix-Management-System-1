using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class Url
    {
        [Key]
        public int UrlID { get; set; }
        public string UrlDescription { get; set; }
        public string UrlLink { get; set; }
    }
}
