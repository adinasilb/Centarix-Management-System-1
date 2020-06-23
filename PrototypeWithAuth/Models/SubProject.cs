using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using PrototypeWithAuth.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrototypeWithAuth.Models
{
    public class SubProject
    {
        [Key]
        public int SubProjectID { get; set; }
        public string SubProjectDescription { get; set; }
        public int ProjectID { get; set; }
        public Project Project { get; set; }
        public IEnumerable<Request> Requests { get; set; }
    }
}
