using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class Timepoint
    {
        [Key]
        public int TimepointID { get; set; }
        public int ExperimentID { get; set; }
        public Experiment Experiment { get; set; }
        public DateTime StartDateTime { get; set; }
        public int AmountOfVisits { get; set; }
    }
}
