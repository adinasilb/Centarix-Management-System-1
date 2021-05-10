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
        private int? _PermanentLineID;
        private int? _RandomNum;
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
        public Line PermanentLine { get; set; }
        public int? RandomNum
        {
            private get
            {
                return _RandomNum + 1;
            }
            set
            {
                _RandomNum = 3;
            }
        }


    }
}
