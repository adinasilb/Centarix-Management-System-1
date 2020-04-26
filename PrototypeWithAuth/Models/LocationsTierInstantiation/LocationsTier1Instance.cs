using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using PrototypeWithAuth.Data;
using System.ComponentModel.DataAnnotations.Schema;
using PrototypeWithAuth.Models;

namespace PrototypeWithAuth.Models.LocationsTierInstantiation
{
    public class LocationsTier1Instance
    {
        [Key]
        public int LocationsTier1InstanceID { get; set; }

        public int LocationsTier1ModelID { get; set; }
        public LocationsTier1Model LocationsTier1Model { get; set; }

        public IEnumerable<LocationsTier2Instance> LocationsTier2Instances { get; set; }

        public string LocationsTier1InstanceAbrv { get; set; }
    }
}
