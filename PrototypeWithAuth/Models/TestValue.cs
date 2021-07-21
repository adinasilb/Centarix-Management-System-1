using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class TestValue
    {
        [Key]
        public int TestValueID { get; set; }
        public int TestHeaderID { get; set; }
        public TestHeader TestHeader { get; set; }
        public int ParticipantID { get; set; }
        public Participant Participant { get; set; }
    }
}
