using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Data.SeededData
{
    public class ProtocolTypeData
    {
        public static List<ProtocolType> Get()
        {
            List<ProtocolType> list = new List<ProtocolType>();
            list.Add(new ProtocolType
            {
                ProtocolTypeID = 1,
                ProtocolTypeDescription = "Research"
            });
            list.Add(new ProtocolType
            {
                ProtocolTypeID = 2,
                ProtocolTypeDescription = "Kit"
            });
            list.Add(new ProtocolType
            {
                ProtocolTypeID = 3,
                ProtocolTypeDescription = "SOP"
            });
            list.Add(new ProtocolType
            {
                ProtocolTypeID = 4,
                ProtocolTypeDescription = "Buffer"
            });
            list.Add(new ProtocolType
            {
                ProtocolTypeID = 5,
                ProtocolTypeDescription = "Robiotic"
            });
            list.Add(new ProtocolType
            {
                ProtocolTypeID = 6,
                ProtocolTypeDescription = "Maintenance"
            });
            return list;
        }

    }
}
