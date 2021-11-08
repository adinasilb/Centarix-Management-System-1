using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Data.SeededData
{
    public static class PaymentTypeData
    {
        public static List<ProtocolType> Get()
        {
            List<ProtocolCategory> list = new List<ProtocolCategory>();
            list.Add(new ProtocolCategory
            {
                ProtocolCategoryTypeID = 1,
                ProtocolCategoryDescription = "Rejuvenation"
            });
            list.Add(new ProtocolCategory
            {
                ProtocolCategoryTypeID = 2,
                ProtocolCategoryDescription = "Biomarkers"
            });
            list.Add(new ProtocolCategory
            {
                ProtocolCategoryTypeID = 3,
                ProtocolCategoryDescription = "Delivery Systems"
            });

            return list;
        }

    }
}
