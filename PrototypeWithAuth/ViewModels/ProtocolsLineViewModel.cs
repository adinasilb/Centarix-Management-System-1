using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class ProtocolsLineViewModel
    {
        public int Index { get; set; }
        public TempLine TempLine { get; set; }
        public string LineNumberString { get; set; }
        public List<LineType> LineTypes { get; set; }
    }
}
