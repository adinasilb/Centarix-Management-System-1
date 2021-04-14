using Microsoft.AspNetCore.Mvc;
using PrototypeWithAuth.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class Protocol
    {
        [Key]
        public int ProtocolID { get; set; }
        public string Name { get; set; }
        public string UniqueCode { get; set; }
        public string VersionNumber { get; set; }
        public IEnumerable<TagProtocol> TagProtocols {get; set;}
        public string ShortDescription { get; set; }
        public string Theory { get; set; }
        public IEnumerable<Url> Urls { get; set; }
        public IEnumerable<MaterialProtocol> MaterialProtocols { get; set; }
        public IEnumerable<Line> Line { get; set; }
        public int ApplicationUserCreatorID { get; set; }
        public ApplicationUser ApplicationUserCreator { get; set; }
        public int ProtocolSubCategoryID { get; set; }
        public ProtocolSubCategory ProtocolSubCategory { get; set; }
        public int ProtocolTypeID { get; set; }
        public ProtocolType ProtocolType { get; set; }
    }
}
