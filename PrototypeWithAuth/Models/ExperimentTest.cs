using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class ExperimentTest
    {
        public int ExperimentID { get; set; }
        public Experiment Experiment { get; set; }
        public int TestID { get; set; }
        public Test Test { get; set; }
    }
}
