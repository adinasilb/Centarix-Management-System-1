using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Models;

namespace PrototypeWithAuth.ViewModels
{
    public class _InternalCalibrationViewModel : ViewModelBase
    {
        public int RequestID { get; set; }
        public InternalCalibration InternalCalibration { get; set; }
        public int InternalCalibrationIndex { get; set; }
        public bool IsNew { get; set; }
    }
}
