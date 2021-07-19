using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class TestFieldHeader
    {
        public TestFieldHeader(int testID, string fieldNames, string fieldTypes, string fieldList)
        {
            TestID = testID;
            FieldNames = fieldNames;
            FieldTypes = fieldTypes;
            FieldList = fieldList;
        }

        [Key]
        public int TestFieldHeaderID { get; set; }
        public int TestID { get; private set; }
        public Test Test { get; set; }
        public string FieldNames { get; private set; }
        public string FieldTypes { get; private set; }
        public string FieldList { get; private set; }

    }
}
