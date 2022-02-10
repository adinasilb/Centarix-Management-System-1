using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class SettingsForm
    {
        public CategoryBase Category { get; set; }
        public int ItemCount { get; set; }
        public int RequestCount { get; set; }
    }
}
