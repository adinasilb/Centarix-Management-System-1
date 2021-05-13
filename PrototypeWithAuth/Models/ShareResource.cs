using PrototypeWithAuth.AppData;
using PrototypeWithAuth.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class ShareResource
    {
        private DateTime _timestamp { get; set; }
        [Key]
        public int ShareResourceID { get; set; }
        public int ResourceID { get; set; }
        public Resource Resource { get; set; }
        public string FromApplicationUserID { get; set; }
        [ForeignKey("FromApplicationUserID")]
        public ApplicationUser FromApplicationUser { get; set; }
        public string ToApplicationUserID { get; set; }
        [ForeignKey("ToApplicationUserID")]
        public ApplicationUser ToApplicationUser { get; set; }

        [DataType(DataType.Date)]
        public DateTime TimeStamp
        {
            get => _timestamp == new DateTime() ? AppUtility.ElixirDate() : _timestamp;
            set { _timestamp = value; }
        }
    }
}
