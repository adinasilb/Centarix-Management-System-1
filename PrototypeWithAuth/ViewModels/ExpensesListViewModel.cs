using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;

namespace PrototypeWithAuth.ViewModels
{
    public class ExpensesListViewModel
    {
        List<ParentCategory> ParentCategories { get; set; }
        List<IDictionary<DateTime, Double>> RequestSumsByMonthYear { get; set; } //change to dictionary
    }
}
