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
    public class LocationsTier2Instance
    {
        [Key]
        public int LocationsTier2InstanceID { get; set; }

        //get foreign key to type tier 2
        public int LocationsTier2ModelID { get; set; }
        public LocationsTier2Model LocationsTier2Model { get; set; }

        //get foreign key to instance type 1
        public int LocationsTier1InstanceID { get; set; }
        public LocationsTier1Instance LocationsTier1Instance { get; set; }

        public IEnumerable<LocationsTier3Instance> LocationsTier3Instances { get; set; }

        public string LocationsTier2InstanceAbrv { get; set; }
    }
}
