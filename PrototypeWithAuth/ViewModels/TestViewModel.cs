using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class TestViewModel
    {
        public ExperimentEntry ExperimentEntry { get; set; }
        public List<Test> Tests { get; set; }
        public List<TestValue> TestValues { get; set; } //THIS IS ONLY FOR THE FIRST ONE!!!!
    }
}
