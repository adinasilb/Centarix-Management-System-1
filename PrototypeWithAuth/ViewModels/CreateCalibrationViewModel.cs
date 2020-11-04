using Microsoft.AspNetCore.Http;
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
        public List<string> ManualFileStrings { get; set; }
        public List<string> WarrantyFileStrings { get; set; }
        public List<string> PicturesFileStrings { get; set; }
        public List<string> MoreFileStrings { get; set; }


        //The PDFs that are passed into the controller:
        public List<IFormFile> ManualFiles { get; set; } //this needs to be changed b/c it is the pdf created by the order
        public List<IFormFile> WarrantyFiles { get; set; }
        public List<IFormFile> PitcuresFiles { get; set; }
        public List<IFormFile> MoreFiles { get; set; }

    }
}
