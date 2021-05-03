using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static PrototypeWithAuth.AppData.AppUtility;

namespace PrototypeWithAuth.ViewModels
{
    public class EntryExitViewModel : ViewModelBase
    {
        public EntryExitEnum EntryExitEnum { get; set; }
        public DateTime? Entry { get; set; }
        public string OffDayRemoved { get; set; }
        public List<TimekeeperNotification> TimekeeperNotifications { get; set; }
    }
}
