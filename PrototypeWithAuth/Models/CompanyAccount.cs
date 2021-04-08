using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using PrototypeWithAuth.Data;
using System.ComponentModel.DataAnnotations.Schema;


namespace PrototypeWithAuth.Models
{
    public class CompanyAccount
    {
        [Key]
        public int CompanyAccountID { get; set; }
        public string CompanyBankNum { get; set; }
        public string CompanyBankName { get; set; }
        public string CompanyBranchNum { get; set; }
        public IEnumerable<CreditCard> CreditCards { get; set; }
    }
}
