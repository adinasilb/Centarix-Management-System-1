using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.Models;

namespace PrototypeWithAuth.ViewModels
{
    public class ResourceAPIViewModel
    {
        public bool Success { get; set; }
        public Resource Resource { get; set; }
    }
}
