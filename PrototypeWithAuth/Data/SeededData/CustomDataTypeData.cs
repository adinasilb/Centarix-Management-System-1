using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Data.SeededData
{
    public class CustomDataTypeData
    {
        public static List<CustomDataType> Get()
        {
            List<CustomDataType> CustomDataTypes = new List<CustomDataType>();
            CustomDataTypes.Add(new CustomDataType()
            {
                CustomDataTypeID = 1,
                Name = "String"
            });
            CustomDataTypes.Add(new CustomDataType()
            {
                CustomDataTypeID = 2,
                Name = "Double"
            });
            CustomDataTypes.Add(new CustomDataType()
            {
                CustomDataTypeID = 3,
                Name = "Bool"
            });
            CustomDataTypes.Add(new CustomDataType()
            {
                CustomDataTypeID = 4,
                Name = "DateTime"
            });
            CustomDataTypes.Add(new CustomDataType()
            {
                CustomDataTypeID = 5,
                Name = "File"
            });
            return CustomDataTypes;
        }
    }
}
