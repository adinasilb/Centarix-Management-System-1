using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class AddMaterialViewModel : ViewModelBase
    {
        public Material Material { get; set; }
        public List<String> FileStrings { get; set; }

    }
}
