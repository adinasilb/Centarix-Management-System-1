using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class Experiment
    {
        [Key]
        public int ExperimentID { get; set; }
        public string ExperimentCode { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string Description { get; set; } //make this an ntext
        public int NumberOfParticipants { get; set; }
        public int MinimumAge { get; set; }
        public int MaximumAge { get; set; }
        public int AmountOfVisits { get; set; }
        public IEnumerable<Timepoint> Timepoints { get; set; }
        public IEnumerable<Participant> Participants { get; set; }
        public IEnumerable<ExperimentTest> ExperimentTests { get; set; }
    }
}
