using PrototypeWithAuth.AppData;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Data.SeededData
{
    public class OrderMethodData
    {
        public static List<OrderMethod> Get()
        {
            List<OrderMethod> list = new List<OrderMethod>();
            list.Add(new OrderMethod
            {
                ID = -1,
                Description = "None",
                DescriptionEnum =AppUtility.OrderMethod.None.ToString()
            });
            list.Add(new OrderMethod
            {
                ID = 1,
                Description = "Add To Cart",
                DescriptionEnum = AppUtility.OrderMethod.AddToCart.ToString()
            });
            list.Add(new OrderMethod
            {
                ID = 2,
                Description = "Order Now",
                DescriptionEnum = AppUtility.OrderMethod.OrderNow.ToString()
            });
            list.Add(new OrderMethod
            {
                ID = 3,
                Description = "Already Purchased",
                DescriptionEnum = AppUtility.OrderMethod.AlreadyPurchased.ToString()
            });
            list.Add(new OrderMethod
            {
                ID = 4,
                Description = "Request Price Quote",
                DescriptionEnum = AppUtility.OrderMethod.RequestPriceQuote.ToString()
            });
            list.Add(new OrderMethod
            {
                ID = 5,
                Description = "Save",
                DescriptionEnum = AppUtility.OrderMethod.Save.ToString()
            });
            list.Add(new OrderMethod
            {
                ID = 6,
                Description = "Excel Upload",
                DescriptionEnum = AppUtility.OrderMethod.ExcelUpload.ToString()
            });
            return list;
        }
    }
}
