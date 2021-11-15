using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Data.SeededData
{
    public class RequestStatusData
    {
        public static List<RequestStatus> Get()
        {
            List<RequestStatus> list = new List<RequestStatus>();
            list.Add(new RequestStatus
            {
                RequestStatusID = 1,
                RequestStatusDescription = "New" // request page, under new
            });
            list.Add(new RequestStatus
            {
                RequestStatusID = 2,
                RequestStatusDescription = "Ordered" // request page, under order
            });
            list.Add(new RequestStatus
              {
                  RequestStatusID = 3,
                  RequestStatusDescription = "RecievedAndIsInventory"  // request page, under recieved (only pass in the first 50) and under Inventory
              });
            // new RequestStatus
            // {
            //     RequestStatusID = 4,
            //     RequestStatusDescription = "Partial" // request page, under order
            // },
            //  new RequestStatus
            //  {
            //      RequestStatusID = 5,
            //      RequestStatusDescription = "Clarify" // request page, under order
            //  },
            list.Add(new RequestStatus
              {
                  RequestStatusID = 6,
                  RequestStatusDescription = "Approved" // request page, under order
              });
            list.Add(new RequestStatus
             {
                 RequestStatusID = 7,
                 RequestStatusDescription = "Saved To Inventory"
             });
            return list;
        }
    }
}
