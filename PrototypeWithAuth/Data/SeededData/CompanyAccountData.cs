using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Data.SeededData
{
    public static class CompanyAccountData
    {
        public static List<CompanyAccount> Get()
        {
            List<CompanyAccount> list = new List<CompanyAccount>();
            list.Add(new CompanyAccount
            {
                CompanyAccountID = 1,
                CompanyBankName = "Discount"
            });
            list.Add(new CompanyAccount
            {
                CompanyAccountID = 2,
                CompanyBankName = "Mercantile"
            });
            list.Add(new CompanyAccount
            {
                CompanyAccountID = 3,
                CompanyBankName = "Leumi"
            });
            list.Add(new CompanyAccount
            {
                CompanyAccountID = 4,
                CompanyBankName = "Payoneer"
            });
            list.Add(new CompanyAccount
            {
                CompanyAccountID = 5,
                CompanyBankName = "Quartzy Bank"
            });
            return list;
        }
    }
}
