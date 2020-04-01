using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using PrototypeWithAuth.ViewModels;

namespace PrototypeWithAuth.ViewModels
{
    public class ExpensesListViewModel
    {
        public List<MonthlyTotalsViewModel> monthlyTotals { get; set; }
    }
}
