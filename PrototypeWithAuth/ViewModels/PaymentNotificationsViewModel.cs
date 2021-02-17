using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.Models;
using PrototypeWithAuth.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using PrototypeWithAuth.AppData.UtilityModels;

namespace PrototypeWithAuth.ViewModels
{
    public class PaymentNotificationsViewModel : ViewModelBase
    {
        
        public IEnumerable<IEnumerable<ParentRequest>> ParentRequests { get; set; }
        

        public Dictionary<string, int> TitleList { get; set; }
    }
}
