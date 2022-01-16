using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class SettingsInventory
    {
        public IEnumerable<CategoryBase> Categories { get; set; }
        public IEnumerable<CategoryBase> Subcategories { get; set; }
        public SettingsForm SettingsForm { get; set; }
    }
}
