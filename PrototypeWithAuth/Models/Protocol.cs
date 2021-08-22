using Microsoft.AspNetCore.Mvc;
using PrototypeWithAuth.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrototypeWithAuth.Models
{
    public class Protocol
    {
        [Key]
        public int ProtocolID { get; set; }
        public string Name { get; set; }
        public string UniqueCode { get; set; }

        public int ProtocolSubCategoryID { get; set; }
        public ProtocolSubCategory ProtocolSubCategory { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
