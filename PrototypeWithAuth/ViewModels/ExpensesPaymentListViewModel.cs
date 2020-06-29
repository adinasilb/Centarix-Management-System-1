using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.Models;
using PrototypeWithAuth.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using PrototypeWithAuth.ViewModels;

namespace PrototypeWithAuth.ViewModels
{
    public class ExpensesPaymentListViewModel
    {
        public ParentCategory ParentCategory { get; set; }

        public double Total { get; set; }
    }

}

