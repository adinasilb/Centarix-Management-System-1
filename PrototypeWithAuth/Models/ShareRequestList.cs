using PrototypeWithAuth.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class ShareRequestList : ShareBase
    {

        [ForeignKey("ObjectID")]
        public RequestList RequestList { get; set; }
        public bool ViewOnly { get; set; }
    }
}
