using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public abstract class FunctionBase
    {
        [Key]
        public int ID { get; set; }
        public int FunctionTypeID { get; set; }
        public FunctionType FunctionType { get; set; }
        public int? ProtocolID { get; set; }
        public Protocol Protocol { get; set; }
        public int? ProductID { get; set; }
        public Product Product { get; set; }
        public bool IsTemporary { get; set; }
        public bool IsTemporaryDeleted { get; set; }
    }
}
