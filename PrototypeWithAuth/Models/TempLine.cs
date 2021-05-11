using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    [Table("TempLines")]
    public class TempLine : LineBase
    {
        public TempLine ParentLine { get; set; }
 
        public int? PermanentLineID
        {
            get
            {
                if (_PermanentLineID == null)
                {
                    return LineID;
                }
                return _PermanentLineID;
            }
            set { _PermanentLineID = value; }
        }
        private int? _PermanentLineID;
        public Line PermanentLine { get; set; }

    }
}
