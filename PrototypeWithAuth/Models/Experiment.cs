using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.AppData.UtilityModels;

namespace PrototypeWithAuth.Models
{
    public class Experiment : ModelBase
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
        public ListImplementsModelBase<Timepoint> Timepoints { get; set; }
        public ListImplementsModelBase<Participant> Participants { get; set; }
        public ListImplementsModelBase<ExperimentTest> ExperimentTests { get; set; }
    }
}
