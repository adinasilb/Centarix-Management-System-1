using PrototypeWithAuth.AppData;
using PrototypeWithAuth.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class VendorComment : CommentBase
    {
        
        [ForeignKey("ObjectID")]
        public Vendor Vendor { get; set; }
     
    }
}
