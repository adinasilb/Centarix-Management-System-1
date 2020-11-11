using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.Models;

namespace PrototypeWithAuth.ViewModels
{
    public class _ExternalCalibrationViewModel
    {
        public int RequestID { get; set; }
        public ExternalCalibration ExternalCalibration { get; set; }
        public int ExternalCalibrationIndex { get; set; }
        public bool IsNew { get; set; }
    }
}
