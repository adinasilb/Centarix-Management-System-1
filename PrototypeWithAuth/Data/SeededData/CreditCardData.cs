using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Data.SeededData
{
    public class CreditCardData
    {
        public static List<CreditCard> Get()
        {
            List<CreditCard> list = new List<CreditCard>();
            list.Add(new CreditCard
            {
                CreditCardID = 1,
                CompanyAccountID = 2,
                CardNumber = "2543"
            });
            list.Add(new CreditCard
            {
                CreditCardID = 2,
                CompanyAccountID = 2,
                CardNumber = "4694"
            });
            list.Add(new CreditCard
            {
                CreditCardID = 3,
                CompanyAccountID = 2,
                CardNumber = "3485"
            });
            list.Add(new CreditCard
            {
                CreditCardID = 4,
                CompanyAccountID = 2,
                CardNumber = "0054"
            });
            list.Add(new CreditCard
            {
                CreditCardID = 5,
                CompanyAccountID = 1,
                CardNumber = "4971"
            });
            list.Add(new CreditCard
            {
                CreditCardID = 6,
                CompanyAccountID = 1,
                CardNumber = "4424"
            });
            list.Add(new CreditCard
            {
                CreditCardID = 7,
                CompanyAccountID = 1,
                CardNumber = "4432"
            });
            list.Add(new CreditCard
            {
                CreditCardID = 8,
                CompanyAccountID = 3,
                CardNumber = "7972"
            });
            return list;
        }
    }
}
