using Microsoft.AspNetCore.Mvc;
using PrototypeWithAuth.AppData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class ReorderViewModel
    {
        public RequestItemViewModel RequestItemViewModel { get; set; }
        public RequestIndexObject RequestIndexObject { get; set; }
        public string ErrorMessages { get; set; }
    }
}
