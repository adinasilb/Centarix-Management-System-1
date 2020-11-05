using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class CalibrationType
    {
        [Key]
        public int CalibrationTypeID { get; set; }
        public String Description { get; set; }
        public string Icon { get; set; }
    }
}
