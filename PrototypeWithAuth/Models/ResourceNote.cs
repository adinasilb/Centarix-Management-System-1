using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class ResourceNote
    {
        [Key]
        public int ResourceNoteID { get; set; }
        public int ResourceID { get; set; }
        public Resource Resource { get; set; }
        public string Note { get; set; }
    }
}
