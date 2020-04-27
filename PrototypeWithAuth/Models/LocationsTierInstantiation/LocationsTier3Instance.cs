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
    public class LocationsTier3Instance
    {

        [Key]
        public int LocationsTier3InstanceID { get; set; }

        //get foreign key to type tier 2
        public int LocationsTier3ModelID { get; set; }
        public LocationsTier3Model LocationsTier3Model { get; set; }

        //get foreign key to instance type 1
        public int LocationsTier2InstanceID { get; set; }
        public LocationsTier2Instance LocationsTier2Instance { get; set; }
        public IEnumerable<LocationsTier4Instance> LocationsTier4Instances { get; set; }

        public string LocationsTier3InstanceAbrv { get; set; }
    }
}
