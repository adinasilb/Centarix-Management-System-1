using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class TestCategory
    {
        [Key]
        public int TestCategoryID { get; set; }
        public string Description { get; set; }
        public int DivisionID { get; set; }
        public Division Division { get; set; }
        public IEnumerable<Test> Tests { get; set; }
    }
}
