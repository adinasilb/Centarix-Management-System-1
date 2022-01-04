using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.AppData.UtilityModels;

namespace PrototypeWithAuth.Models
{
    public class TestOuterGroup : ModelBase
    {
        [Key]
        public int TestOuterGroupID { get; set; }
        public string Name { get; set; }
        public bool IsNone { get; set; }
        public int TestID { get; set; }
        public Test Test { get; set; }
        public ListImplementsModelBase<TestGroup> TestGroups { get; set; }
        public int SequencePosition { get; set; }
    }
}
