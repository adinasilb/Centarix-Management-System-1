using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class TempReportJson :TempJson
    {
        [Key]
        public Guid TempReportJsonID { get; set; }
    }
}
