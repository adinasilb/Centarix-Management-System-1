using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class Participant
    {
        [Key]
        public int ParticipantID { get; set; }
        public DateTime DOB { get; set; }
        public int GenderID { get; set; }
        public Gender Gender { get; set; }
        public int ParticipantStatusID { get; set; }
        public int ExperimentID { get; set; }
        public Experiment Experiment { get; set; }
    }
}
