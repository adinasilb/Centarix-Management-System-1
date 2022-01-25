﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class RecurringOrder : Product
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TimePeriodID { get; set; }
        public TimePeriod TimePeriod { get; set; }
        public int TimePeriodAmount { get; set; }
        public int Occurances { get; set; }
        public bool HasEnd { get; set; }
    }
}