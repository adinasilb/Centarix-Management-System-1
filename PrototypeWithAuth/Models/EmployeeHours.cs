﻿using System;
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
        [DisplayFormat(DataFormatString = "{0:hh:mm}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Time)]
        public DateTime? Entry1 { get; set; }
        [DisplayFormat(DataFormatString = "{0:hh:mm}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Time)]
        public DateTime? Entry2 { get; set; }
        [DisplayFormat(DataFormatString = "{0:hh:mm}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Time)]
        public DateTime? Exit1 { get; set; }
        [DisplayFormat(DataFormatString = "{0:hh:mm}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Time)]
        public DateTime? Exit2 { get; set; }
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public int? OffDayTypeID { get; set; }
        public OffDayType OffDayType { get; set; }
        public int? EmployeeHoursStatusID { get; set; }
        [DisplayFormat(DataFormatString = "{0:hh:mm}", ApplyFormatInEditMode = true)]
        private TimeSpan? _TotalHours;
        public TimeSpan? TotalHours { 
            get { 
                if(_TotalHours==TimeSpan.Zero)
                    return ((Exit1 - Entry1) ?? TimeSpan.Zero) + ((Exit2 - Entry2) ?? TimeSpan.Zero);
                else return _TotalHours;
                }
            set { _TotalHours = value; }
        }
             
        public EmployeeHoursStatus EmployeeHoursStatus { get; set; } 
        
    }
}
