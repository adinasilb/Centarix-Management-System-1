using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.Models;

namespace PrototypeWithAuth.ViewModels
{
    public class TestValuesViewModel
    {
        public Test Test { get; set; }
        public List<TestValue> TestValues { get; set; }
        public int ListNumber { get; set; }
    }
}
