using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using PrototypeWithAuth.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrototypeWithAuth.Models
{
    public class Payment
    {
        [Key]
        public int PaymentID { get; set; }
        public double Sum { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime PaymentDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime PaymentReferenceDate { get; set; }
        public DateTime PaymentReferenceDate_submit { get { return PaymentReferenceDate; } set { PaymentReferenceDate = value; } }
        public string Reference { get; set; }
        //public int RequestID { get; set; }
        //public Request Request { get; set; }
        public int RequestID { get; set; }
        public Request Request { get; set; }
        public int? CompanyAccountID { get; set; }
        public CompanyAccount CompanyAccount{ get; set; }
        public bool IsDeleted { get; set; }
        public bool IsPaid { get; set; }
    }
}

