using PrototypeWithAuth.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class ProtocolInstance
    {
        [Key]
        public int ProtocolInstanceID { get; set; }
        public string ApplicationUserID { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public int ProtocolID {get; set;}
        public Protocol Protocol { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int CurrentLineID { get; set; }
        public Line CurrentLine { get; set; }
        public string ResultDescription { get; set; }
        public string TemporaryResultDescription { get; set; }
        public bool IsFinished { get; set; }

    }
}
