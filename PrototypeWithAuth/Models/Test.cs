using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.AppData.UtilityModels;

namespace PrototypeWithAuth.Models
{
    public class Test : ModelBase
    {
        [Key]
        public int TestID { get; set; }
        public string Name { get; set; }
        public IEnumerable<ExperimentTest> ExperimentTests { get; set; }
        //public int TestCategoryID { get; set; }
        //public TestCategory TestCategory { get; set; }
        public int SiteID { get; set; }
        public Site Site { get; set; }
        public ListImplementsModelBase<TestOuterGroup> TestOuterGroups { get; set; }
    }
}
