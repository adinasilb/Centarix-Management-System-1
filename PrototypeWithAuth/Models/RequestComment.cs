using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using PrototypeWithAuth.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrototypeWithAuth.Models
{
    public class RequestComment:CommentBase
    {

        [ForeignKey("ObjectID")]
        public Request Request { get; set; }
       
    }
}

