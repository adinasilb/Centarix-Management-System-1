using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.Models;

namespace PrototypeWithAuth.ViewModels
{
    public class FieldViewModel
    {
        public int ListNumber { get; set; }
        public int TestID { get; set; }
        public int TestValueID { get; set; }
        public TestHeader TestHeader { get; set; }
        public AppUtility.DataTypeEnum DataTypeEnum { get; set; }
        public double Double { get; set; }
        public string String { get; set; }
        public DateTime DateTime { get; set; }
        public IFormFile File { get; set; }
        public bool Bool { get; set; }
        public int FieldID { get; set; }
        public AppUtility.DataCalculation DataCalculation { get; set; }
    }
}
