using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PrototypeWithAuth.AppData.UtilityModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PrototypeWithAuth.ViewModels
{
    public class ApplicationUserListViewModel : ViewModelBase
    {
        public IEnumerable<ApplicationIdentity> ApplicationIdentity { get; set; }
    }
}
