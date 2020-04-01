using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using PrototypeWithAuth.Models;

namespace PrototypeWithAuth.ViewModels
{
    public class MonthlyTotalsViewModel
    {

        public int Month { get; set; }
        public int Year { get; set; }
        public double GrandTotal { get; set; }
        public double PlasticsTotal { get; set; }
        public double ReagentsTotal { get; set; }
        public double ProprietyTotal { get; set; }
        public double ReusableTotal { get; set; }
        //public List<Request> ExpensesByCatgeory { get; set; }
        // public ParentCategory ParentCategory { get; set; } //for expense view model
        // public double Total { get; set; }//for expense view model
        //public IEnumerable<ExpensesPaymentListViewModel> TotalsForCat { get; set; }
        // public IEnumerable<ParentCategory> ParentCategories { get; set; }

    }
}
