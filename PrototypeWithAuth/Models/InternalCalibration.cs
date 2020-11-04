using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class InternalCalibration : Calibration
    {
       public int Days { get; set; }
       public int Months { get; set; }
       public bool Done { get; set; }
       public bool IsRepeat { get; set; }
    }
}
