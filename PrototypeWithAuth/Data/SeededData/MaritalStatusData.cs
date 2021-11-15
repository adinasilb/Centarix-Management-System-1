using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Data.SeededData
{
    public class MaritalStatusData
    {
        public static List<MaritalStatus> Get()
        {
            List<MaritalStatus> list = new List<MaritalStatus>();
            list.Add(new MaritalStatus
            {
                MaritalStatusID = 1,
                Description = "Married"
            });
            list.Add(new MaritalStatus
            {
                MaritalStatusID = 2,
                Description = "Single"
            });
            list.Add(new MaritalStatus
            {
                MaritalStatusID = 3,
                Description = "Divorced"
            });
            return list;
        }  
    }
}
