using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class TestOuterGroup
    {
        [Key]
        public int TestOuterGroupID { get; set; }
        public string Name { get; set; }
        public bool IsNone { get; set; }
        public int TestID { get; set; }
        public Test Test { get; set; }
        public List<TestGroup> TestGroups { get; set; }
    }
}
