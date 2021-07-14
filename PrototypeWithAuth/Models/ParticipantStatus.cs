using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class ParticipantStatus
    {
        [Key]
        public int ParticipantStatusID { get; set; }
        public string Description { get; set; }
        public IEnumerable<Participant> Participants { get; set; }
    }
}
