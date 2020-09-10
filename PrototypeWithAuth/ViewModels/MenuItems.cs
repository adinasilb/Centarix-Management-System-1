using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;

namespace PrototypeWithAuth.ViewModels
{
    public class MenuItems
    {
        public string Description { get; set; }
        public string ActionLink { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public RouteValueDictionary RouteValues { get; set; }
        public string Classes { get; set; }
        public string ID { get; set; }
        public string IconName { get; set; }
    }
}
