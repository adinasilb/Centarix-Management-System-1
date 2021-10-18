using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Models;


namespace PrototypeWithAuth.ViewModels
{
    public class HomePageViewModel : ViewModelBase
    {
        public IEnumerable<Menu> Menus {get;set;}
    }
}
