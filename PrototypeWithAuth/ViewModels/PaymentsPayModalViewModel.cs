using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.AppData.UtilityModels;

namespace PrototypeWithAuth.ViewModels
{
    public class PaymentsPayModalViewModel : ViewModelBase
    {
        public List<Request> Requests { get; set; }
        public List<Payment> Payments { get; set; }
        public Payment Payment { get; set; }
        public List<CompanyAccount> CompanyAccounts {get; set;}
        public AppUtility.SidebarEnum AccountingEnum { get; set; }
        public List<PaymentType> PaymentTypes { get; set; }
        public List<CheckboxViewModel> ShippingToPay { get; set; }
        public StringWithBool Error { get; set; }
        public bool AddInvoice { get; set; }
        public InvoiceInfoViewModel InvoiceInfoViewModel { get; set; }
        public bool PartialPayment { get; set; }
        public decimal PartialAmtToPay { get; set; }
        public decimal PercentageToPay { get; set; }
    }
}
