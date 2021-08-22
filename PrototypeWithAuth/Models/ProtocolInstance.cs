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
        public int ProtocolVersionID { get; set; }
        public ProtocolVersion ProtocolVersion { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int CurrentLineID { get; set; }
        public Line CurrentLine { get; set; }
        public string ResultDescription { get; set; }
        public string TemporaryResultDescription { get; set; }
        public bool IsFinished { get; set; }
        public bool ResultsReported { get; set; }
        public IEnumerable<LineChange> LineChange { get ;set;}

    }
}
