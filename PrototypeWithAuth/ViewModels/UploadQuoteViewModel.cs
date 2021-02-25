using PrototypeWithAuth.AppData;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class UploadQuoteViewModel : ViewModelBase
    {
        public ParentQuote ParentQuote { get; set; }
        public List<string> FileStrings { get; set; }
        public AppUtility.OrderTypeEnum OrderTypeEnum { get; set; }
        public RequestIndexObject RequestIndexObject { get; set; }
        public List<int> RequestIDs { get; set; }
    }
}
