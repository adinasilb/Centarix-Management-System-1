using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class Test
    {
        [Key]
        public int TestID { get; set; }
        public string Name { get; set; }
        public int ExperimentID { get; set; }
        public Experiment Experiment { get; set; }
        public int TestCategoryID { get; set; }
        public TestCategory TestCategory { get; set; }
        
        public List<TestOuterGroup> TestOuterGroups { get; set; }
    }
}
