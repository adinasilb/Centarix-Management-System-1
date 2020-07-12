using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class Menu
    {
        [Key]
        public int menuID { get; set; }
        public string MenuDescription { get; set; }
        public string MenuViewName { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string MenuImageURL { get; set; }
    }
}
