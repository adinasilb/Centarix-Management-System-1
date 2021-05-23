using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static PrototypeWithAuth.AppData.AppUtility;

namespace PrototypeWithAuth.Models
{
    public class GlobalInfo
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public String GlobalInfoType  { get ;set;}
    }
}
