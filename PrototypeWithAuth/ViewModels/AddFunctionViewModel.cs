using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class AddFunctionViewModel : ViewModelBase
    {
        public FunctionType FunctionType { get; set; }
        public TempLine Line { get; set; }
    }
}
