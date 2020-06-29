using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using PrototypeWithAuth.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrototypeWithAuth.Models
{
    public class UnitParentType
    {
        [Key]
        public int UnitParentTypeID { get; set; }
        public string UnitParentTypeDescription { get; set; }
        public IEnumerable<UnitType> UnitTypes { get; set; }
    }
}
