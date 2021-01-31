using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class EmployeeHours
    {
        [Key]
        public int EmployeeHoursID { get; set; }
        public string EmployeeID { get; set; }
        [ForeignKey("EmployeeID")]
        public Employee Employee { get; set; }
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Time)]
        public DateTime? Entry1 { get; set; }
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Time)]
        public DateTime? Entry2 { get; set; }
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Time)]
        public DateTime? Exit1 { get; set; }
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Time)]
        public DateTime? Exit2 { get; set; }
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public DateTime Date_submit { get { return Date; } set { Date = value; } }
        public int? OffDayTypeID { get; set; }
        [ForeignKey("OffDayTypeID")]
        public OffDayType OffDayType { get; set; }
        public int? PartialOffDayTypeID { get; set; }
        [ForeignKey("PartialOffDayTypeID")]
        public OffDayType PartialOffDayType { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan? PartialOffDayHours { get; set; }

        private TimeSpan? _TotalHours;
        [DisplayFormat(DataFormatString = @"{0:hh\:mm}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Time)]
        public TimeSpan? TotalHours { 
            get { 
                if(Entry1!=null || Exit1 !=null)
                    return ((Exit1 - Entry1) ?? TimeSpan.Zero) + ((Exit2 - Entry2) ?? TimeSpan.Zero);
                else return _TotalHours;
                }
            set { _TotalHours = value; }

        }
        public int? EmployeeHoursStatusEntry1ID { get; set; }
        [ForeignKey("EmployeeHoursStatusEntry1ID")]
        public EmployeeHoursStatus EmployeeHoursStatusEntry1 { get; set; }
        public int? EmployeeHoursStatusEntry2ID { get; set; }
        [ForeignKey("EmployeeHoursStatusEntry2ID")]
        public EmployeeHoursStatus EmployeeHoursStatusEntry2 { get; set; }
        public int? CompanyDayOffID { get; set; }
        public CompanyDayOff CompanyDayOff { get; set; }
        public bool IsBonus { get; set; }
    }
}
