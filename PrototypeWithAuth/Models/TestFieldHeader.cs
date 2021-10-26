using PrototypeWithAuth.AppData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class TestFieldHeader
    {
        public TestFieldHeader()
        {

        }
        public TestFieldHeader(int testID, List<string> fieldNames, List<string> fieldTypes)
        {
            TestID = testID;
            TempJson fieldNamesTemp = new TempJson();
            JsonExtensions.SerializeViewModel(fieldNamesTemp, fieldNames);
            FieldNames = fieldNamesTemp.Json;
            TempJson fieldTypesTemp = new TempJson();
            JsonExtensions.SerializeViewModel(fieldTypesTemp, fieldTypes);
            FieldTypes = fieldTypesTemp.Json;
        }
        public TestFieldHeader(int testID, List<string> fieldNames, List<string> fieldTypes, List<string> fieldList)
        {
            TestID = testID;
            TempJson fieldNamesTemp = new TempJson();
            JsonExtensions.SerializeViewModel(fieldNamesTemp, fieldNames);
            FieldNames = fieldNamesTemp.Json;
            TempJson fieldTypesTemp = new TempJson();
            JsonExtensions.SerializeViewModel(fieldTypesTemp, fieldTypes);
            FieldTypes = fieldTypesTemp.Json;
            TempJson fieldListTemp = new TempJson();
            JsonExtensions.SerializeViewModel(fieldListTemp, fieldList);
            FieldList = fieldNamesTemp.Json;
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
