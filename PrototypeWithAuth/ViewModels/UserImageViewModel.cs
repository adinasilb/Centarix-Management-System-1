using Microsoft.AspNetCore.Http;
using PrototypeWithAuth.AppData.UtilityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class UserImageViewModel : ViewModelBase
    {
        public IFormFile FileToSave { get; set; }
    }
}
