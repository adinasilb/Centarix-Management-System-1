﻿using PrototypeWithAuth.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class ProtocolVersion : ModelBase
    {
        [Key]
        public int ProtocolVersionID { get; set; }
        public int VersionNumber { get; set; }
        //public IEnumerable<TagProtocol> TagProtocols { get; set; }
        public string ShortDescription { get; set; }
        public string Theory { get; set; }
        public List<Link> Urls { get; set; }
        public IEnumerable<Material> Materials { get; set; }
        public IEnumerable<Line> Lines { get; set; }
        public string ApplicationUserCreatorID { get; set; }
        public ApplicationUser ApplicationUserCreator { get; set; }
        public DateTime CreationDate { get; set; }
        public IEnumerable<ProtocolInstance> ProtocolInstances { get; set; }
        public int ProtocolID { get; set; }
        public Protocol Protocol { get; set; }

        public string Name { get
            {
                if (Protocol != null)
                {
                    return Protocol.Name;
                }
                else
                {
                    return "Error: need to include protocol";
                }
            }
            set {; } }

    }
}
