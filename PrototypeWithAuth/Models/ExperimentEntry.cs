using PrototypeWithAuth.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class ExperimentEntry
    {
        [Key]
        public int ExperimentEntryID { get; set; }
        public DateTime DateTime { get; set; }
        public int ParticipantID { get; set; }
        public Participant Participant { get; set; }
        public int VisitNumber { get; set; }
        public string ApplicationUserID { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public int SiteID { get; set; }
        public Site Site { get; set; }
    }
}
