using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class Gender
    {
        [Key]
        public int GenderID { get; set; }
        public string Description { get; set; }
        public IEnumerable<Participant> Participants { get; set; }
    }
}
