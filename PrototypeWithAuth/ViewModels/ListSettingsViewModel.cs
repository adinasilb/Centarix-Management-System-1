using Org.BouncyCastle.Asn1.Mozilla;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.AppData.UtilityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;
using PrototypeWithAuth.Models;

namespace PrototypeWithAuth.ViewModels
{
    public class ListSettingsViewModel : ViewModelBase
    {
        public List<RequestList> RequestLists { get; set; }
        public RequestList SelectedList { get; set; }
        public bool ReloadModal { get; set; }
    }
}
