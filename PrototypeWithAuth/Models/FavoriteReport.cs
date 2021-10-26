using PrototypeWithAuth.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class FavoriteReport:FavoriteBase
    {
        public int? ReportID { get; set; }
        public Report Report { get; set; }

    }
}
