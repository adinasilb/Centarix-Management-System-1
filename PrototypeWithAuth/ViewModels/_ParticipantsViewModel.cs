using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class _ParticipantsViewModel
    {
        public Experiment Experiment { get; set; }
        public List<TDViewModel> Headers { get; set; }
        public List<List<TDViewModel>> Rows { get; set; }
    }
}
