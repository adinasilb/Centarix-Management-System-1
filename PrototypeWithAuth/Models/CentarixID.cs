using PrototypeWithAuth.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class CentarixID
    {
        private int _EmployeeStatusID;
        private Employee _Employee;

        [Key]
        public int CentarixIDID { get; set; }

        public string EmployeeID { get; set; }

        [ForeignKey("EmployeeID")]
        public Employee Employee
        {
            get
            {
                return _Employee;
            }
            set
            {
                _EmployeeStatusID = value.EmployeeStatusID;
                _Employee = value;
            }
        }

        public string CentarixIDNumber { get; set; }
        public bool IsCurrent { get; set; }
        public DateTime TimeStamp { get; set; }
        public int EmployeeStatusID
        {
            get
            {
                return _EmployeeStatusID;
            }
            private set
            {
                _EmployeeStatusID = value;
            }
        }
        [ForeignKey("EmployeeStatusID")]
        public EmployeeStatus EmployeeStatus { get; set; }
    }
}
