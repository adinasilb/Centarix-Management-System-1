using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class TestGroup
    {
        [Key]
        public int TestGroupID { get; set; }
        public string Name { get; set; }
        public bool IsNone { get; set; }
        public int TestOuterGroupID { get; set; }
        public TestOuterGroup TestOuterGroup { get; set; }
        public List<TestHeader> TestHeaders { get; set; }
        public int SequencePosition { get; set; }

    }
}
