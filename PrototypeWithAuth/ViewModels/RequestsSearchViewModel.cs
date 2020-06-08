﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.Models;

namespace PrototypeWithAuth.ViewModels
{
    public class RequestsSearchViewModel
    {
        public IEnumerable<ParentCategory> ParentCategories { get; set; }
        public IEnumerable<Vendor> Vendors { get; set; }
        public Request Request { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        

        public IQueryable<Request> ReturnRequests { get; set; }
    }
}
