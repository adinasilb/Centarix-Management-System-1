using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Data.SeededData
{
    public static class PaymentStatusData
    {
        public static List<PaymentStatus> Get()
        {
            List<PaymentStatus> list = new List<PaymentStatus>();
            list.Add(new PaymentStatus
            {
                PaymentStatusID = 2,
                PaymentStatusDescription = "+ 30"
            });
            list.Add(new PaymentStatus
            {
                PaymentStatusID = 3,
                PaymentStatusDescription = "Pay Now"
            });
            list.Add(new PaymentStatus
            {
                PaymentStatusID = 4,
                PaymentStatusDescription = "Pay Upon Arrival"
            });
            list.Add(new PaymentStatus
            {
                PaymentStatusID = 5,
                PaymentStatusDescription = "Installments"
            });
            list.Add(new PaymentStatus
            {
                PaymentStatusID = 7,
                PaymentStatusDescription = "Standing Order"
            });
            list.Add(new PaymentStatus
            {
                PaymentStatusID = 8,
                PaymentStatusDescription = "Specify Payment Later"
            });

            return list;
        }
    }
}
