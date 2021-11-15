using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Data.SeededData
{
    public static class PaymentTypeData
    {
        public static List<PaymentType> Get()
        {
            List<PaymentType> list = new List<PaymentType>();
            list.Add(new PaymentType
            {
                PaymentTypeID = 1,
                PaymentTypeDescription = "Credit Card"

            });
            list.Add(new PaymentType
            {
                PaymentTypeID = 2,
                PaymentTypeDescription = "Check"
            });
            list.Add(new PaymentType
            {
                PaymentTypeID = 3,
                PaymentTypeDescription = "Wire"
            });

            return list;
        }

    }
}
