using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.Models;
using PrototypeWithAuth.AppData.UtilityModels;

namespace PrototypeWithAuth.ViewModels
{
    public class _ExternalCalibrationViewModel : ViewModelBase
    {
        public int RequestID { get; set; }
        public ExternalCalibration ExternalCalibration { get; set; }
        public int ExternalCalibrationIndex { get; set; }
        public bool IsNew { get; set; }
    }
}
