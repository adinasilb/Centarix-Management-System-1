using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.Models;

namespace PrototypeWithAuth.ViewModels
{
    public class EntriesViewModel
    {
        public Participant Participant { get; set; }
        public List<TDViewModel> EntryHeaders { get; set; }
        public List<List<TDViewModel>> EntryRows { get; set; }
    }
}
