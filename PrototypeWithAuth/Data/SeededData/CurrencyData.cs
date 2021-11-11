using PrototypeWithAuth.AppData;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Data.SeededData
{
    public static class CurrencyData
    {
        public static IEnumerable<Currency> GetCurrencies()
        {
            var currencies = new List<Currency>()
            {
                new Currency()
                {
                    CurrencyID = -1,
                    CurrencyName = AppUtility.CurrencyEnum.None.ToString()
                },
                new Currency()
                {
                    CurrencyID = 1,
                    CurrencyName = AppUtility.CurrencyEnum.USD.ToString()
                },
                new Currency()
                {
                    CurrencyID = 2,
                    CurrencyName = AppUtility.CurrencyEnum.NIS.ToString()
                }
            };
            return currencies;
        }
    }
}
