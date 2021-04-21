using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class CreateProtocolsViewModel
    {
        public Protocol Protocol { get; set; }
        public List<String> Tags { get; set; }
    }
}
