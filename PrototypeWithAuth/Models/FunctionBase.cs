using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public  class FunctionBase
    {
        [Key]
        public int ID { get; set; }
        public int FunctionTypeID { get; set; }
        public FunctionType FunctionType { get; set; }
        public int? ProtocolVersionID { get; set; }
        public ProtocolVersion ProtocolVersion { get; set; }
        public int? ProductID { get; set; }
        public Product Product { get; set; }
        public TimeSpan Timer { get; set; }
        public string Description { get; set; }
        public bool IsTemporaryDeleted { get; set; }
    }
}
