using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using PrototypeWithAuth.Data;
using System.ComponentModel.DataAnnotations.Schema;
using PrototypeWithAuth.AppData;

namespace PrototypeWithAuth.Models
{
    public class ProtocolComment : CommentBase
    {

        [ForeignKey("ObjectID")]
        public Protocol Protocol { get; set; }

         }
}
