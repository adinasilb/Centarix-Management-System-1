using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class TempLinesJson 
    {
        public int TempLinesJsonID { get; set; }
        public string Json { get; set; }
        public Guid UniqueGuid { get; set; }
    }
}
