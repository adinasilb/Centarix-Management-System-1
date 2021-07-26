using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class ExperimentViewModel
    {
        public Experiment Experiment { get; set; }
        public _ParticipantsViewModel _ParticipantsViewModel { get; set; }
    }
}
