using PrototypeWithAuth.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public abstract class ShareBase
    {
        private DateTime _timestamp { get; set; }
        [Key]
        public int ShareID { get; set; }
        public string FromApplicationUserID { get; set; }
        [ForeignKey("FromApplicationUserID")]
        public ApplicationUser FromApplicationUser { get; set; }
        public string ToApplicationUserID { get; set; }
        [ForeignKey("ToApplicationUserID")]
        public ApplicationUser ToApplicationUser { get; set; }

        [DataType(DataType.Date)]
        public DateTime TimeStamp
        {
            get => _timestamp == new DateTime() ? DateTime.Now : _timestamp;
            set { _timestamp = value; }
        }
    }
}
