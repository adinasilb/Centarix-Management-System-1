using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.Models;

namespace PrototypeWithAuth.ViewModels
{
    public class AddParticipantViewModel
    {
        public Participant Participant { get; set; }
        public List<Gender> Genders { get; set; }
    }
}
