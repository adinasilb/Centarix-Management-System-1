using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.AppData.UtilityModels;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace PrototypeWithAuth.ViewModels
{
    public class PaymentsInvoiceViewModel : ViewModelBase
    {
        public List<Request> Requests { get; set; }
        public Payment Payment { get; set; }
        public List<CompanyAccount> CompanyAccounts {get; set;}
        public AppUtility.SidebarEnum AccountingEnum { get; set; }
        public List<PaymentType> PaymentTypes { get; set; }
        public Invoice Invoice { get; set; }
        [Display(Name = "Invoice Image")]
        public IFormFile InvoiceImage { get; set; }
        public string InvoiceImageSaved { get; set; }
        public decimal AmtLeftToPay { get; set; }
        public List<CheckboxViewModel> ShippingToPay { get; set; }

    }
}
