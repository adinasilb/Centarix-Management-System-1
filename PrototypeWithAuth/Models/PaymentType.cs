using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using PrototypeWithAuth.Data;
using System.ComponentModel.DataAnnotations.Schema;


namespace PrototypeWithAuth.Models
{
    public class PaymentType
    {
        [Key]
        public int PaymentTypeID { get; set; }
        public string PaymentTypeDescription { get; set; }
    }
}
