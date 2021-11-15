using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Data.SeededData
{
    public class ProtocolSubCategoryData
    {
        public static List<ProtocolSubCategory> Get()
        {
            List<ProtocolSubCategory> list = new List<ProtocolSubCategory>();
            list.Add(new ProtocolSubCategory
            {
                ProtocolSubCategoryTypeID = 1,
                ProtocolCategoryTypeID = 1,
                ProtocolSubCategoryTypeDescription = "Telomeres "
            });
            list.Add(new ProtocolSubCategory
            {
                ProtocolSubCategoryTypeID = 2,
                ProtocolCategoryTypeID = 1,
                ProtocolSubCategoryTypeDescription = "Epigenetics"
            });
            list.Add(new ProtocolSubCategory
            {
                ProtocolSubCategoryTypeID = 3,
                ProtocolCategoryTypeID = 2,
                ProtocolSubCategoryTypeDescription = "Telomeres "
            });
            list.Add(new ProtocolSubCategory
            {
                ProtocolSubCategoryTypeID = 4,
                ProtocolCategoryTypeID = 2,
                ProtocolSubCategoryTypeDescription = "Transcription"
            });
            list.Add(new ProtocolSubCategory
            {
                ProtocolSubCategoryTypeID = 5,
                ProtocolCategoryTypeID = 2,
                ProtocolSubCategoryTypeDescription = "Methylation"
            });
            list.Add(new ProtocolSubCategory
            {
                ProtocolSubCategoryTypeID = 6,
                ProtocolCategoryTypeID = 3,
                ProtocolSubCategoryTypeDescription = "AAV"
            });
          
            return list;
        }
        }
}
