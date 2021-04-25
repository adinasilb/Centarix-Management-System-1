using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class ProtocolsDocumentsViewModel :DocumentsModalViewModel
    {
        public List<Protocol> Protocols { get; set; }
    }
}
