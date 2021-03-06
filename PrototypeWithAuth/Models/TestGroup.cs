using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.AppData.UtilityModels;

namespace PrototypeWithAuth.Models
{
    public class TestGroup : ModelBase
    {
        [Key]
        public int TestGroupID { get; set; }
        public string Name { get; set; }
        public bool IsNone { get; set; }
        public int TestOuterGroupID { get; set; }
        public TestOuterGroup TestOuterGroup { get; set; }
        public ListImplementsModelBase<TestHeader> TestHeaders { get; set; }
        public int SequencePosition { get; set; }

    }
}
