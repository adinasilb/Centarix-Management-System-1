using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class AddResultsFunctionViewModel : AddFunctionViewModel<FunctionResult>
    {
        public string ClosingTags { get; set; }
     
    }
}
