using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.Models;
using PrototypeWithAuth.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace PrototypeWithAuth.ViewModels
{
    public class PaymentNotificationsViewModel
    {
        
        public IEnumerable<IEnumerable<ParentRequest>> ParentRequests { get; set; }
        

        public Dictionary<string, int> TitleList { get; set; }
    }
}
