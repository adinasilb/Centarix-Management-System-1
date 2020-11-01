using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class CreateCalibrationViewModel
    {
        public String ProductDescription { get; set; }
        public Repair Repair { get; set; }
        public List<InternalCalibration> InternalCalibration {get; set;}
        public List<ExternalCalibration> ExternalCalibrations { get; set; }
        public List<Calibration> PastCalibrations { get; set; }
        public int RequestID { get; set; }
    }
}
