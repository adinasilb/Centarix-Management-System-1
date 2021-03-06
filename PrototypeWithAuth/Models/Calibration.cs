using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class Calibration : ModelBase
    {
        [Key]
        public int CalibrationID { get; set; }
        public string CalibrationName { get; set; }
        public int RequestID { get; set; }
        public Request Request { get; set; }
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public int CalibrationTypeID {get; set;}
        public CalibrationType CalibrationType {get; set;}
        public string Code { get; set; }
        public string Description { get; set; }
        public int Days { get; set; }
        public int Months { get; set; }
        public bool Done { get; set; }
        public bool IsRepeat { get; set; }
        public bool IsDeleted { get; set; }
    }
}
