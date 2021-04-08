using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class CreditCard
    {
        [Key]
        public int CreditCardID { get; set; }
        [Required]
        public int CompanyAccountID { get; set; }
        public CompanyAccount CompanyAccount { get; set; }
        [MaxLength(4)]
        public string CardNumber { get; set; }
    }
}
