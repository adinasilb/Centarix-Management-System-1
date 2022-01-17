using PrototypeWithAuth.AppData;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Data.SeededData
{
    public class OrderTypeData
    {
        public static List<OrderType> Get()
        {
            List<OrderType> list = new List<OrderType>();
            list.Add(new OrderType
            {
                ID = 1,
                Description = "Single",
                DescriptionEnum =AppUtility.OrderType.Single.ToString()
            });
            list.Add(new OrderType
            {
                ID = 2,
                Description = "Recurring",
                DescriptionEnum = AppUtility.OrderType.Recurring.ToString()
            });
            list.Add(new OrderType
            {
                ID = 3,
                Description = "Standing",
                DescriptionEnum = AppUtility.OrderType.Standing.ToString()
            });
            return list;
        }
    }
}
