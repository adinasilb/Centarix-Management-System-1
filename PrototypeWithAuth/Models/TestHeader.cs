using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class TestHeader : ModelBase
    {
        [Key]
        public int TestHeaderID { get; set; }
        public int TestGroupID { get; set; }
        public TestGroup TestGroup { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string List { get; set; }
        public int SequencePosition { get; set; }
        public bool IsSkip { get; set; }
        public string Calculation { get; set; }
    }
}
