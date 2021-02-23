using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore.Query.Internal;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class NotificationsListViewModel : ViewModelBase
    {
        public Dictionary<Vendor, List<ParentRequestListViewModel>> ParentRequestList { get; set; }

    }
}
