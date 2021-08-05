using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PrototypeWithAuth.AppData;

namespace PrototypeWithAuth.ViewModels
{
    public class FieldViewModel
    {
        public int TestID { get; set; }
        public string Header { get; set; }
        public AppUtility.DataTypeEnum DataTypeEnum { get; set; }
        public double Double { get; set; }
        public string String { get; set; }
        public DateTime DateTime { get; set; }
        public IFormFile File { get; set; }
        public bool Bool { get; set; }
        public int FieldID { get; set; }
    }
}
