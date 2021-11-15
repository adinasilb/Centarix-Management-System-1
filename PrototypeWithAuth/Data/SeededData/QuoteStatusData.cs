using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Data.SeededData
{
    public class QuoteStatusData
    {
        public static List<QuoteStatus> Get()
        {
            List<QuoteStatus> list = new List<QuoteStatus>();
            list.Add(new QuoteStatus
            {
                QuoteStatusID = -1,
                QuoteStatusDescription = "NoStatus" // request page, under reorder
            });
            list.Add(new QuoteStatus
            {
                QuoteStatusID = 1,
                QuoteStatusDescription = "AwaitingRequestOfQuote" // request page, under reorder
            });
            list.Add(new QuoteStatus
            {
                QuoteStatusID = 2,
                QuoteStatusDescription = "AwaitingQuoteResponse" // lab quote manange page, under quotes
            });
            list.Add(new QuoteStatus
            {
                QuoteStatusID = 3,
                QuoteStatusDescription = "AwaitingQuoteOrder" // lab quote manange page, under quotes
            });
            list.Add(new QuoteStatus
            {
                QuoteStatusID = 4,
                QuoteStatusDescription = "QuoteRecieved"
            });
            return list;
        }
    }
}
