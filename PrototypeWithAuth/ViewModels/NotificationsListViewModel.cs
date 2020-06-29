using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class NotificationsListViewModel
    {
        public IEnumerable<ParentRequest> ParentRequests { get; set; }
    }
}
