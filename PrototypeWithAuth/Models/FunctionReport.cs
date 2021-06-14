using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class FunctionReport: FunctionBase
    {
        [ForeignKey ("TextID")]
        public Report Report { get; set; }
    }
}
