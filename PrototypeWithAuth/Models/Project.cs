using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using PrototypeWithAuth.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrototypeWithAuth.Models
{
    public class Project
    {
        [Key]
        public int ProjectID { get; set; }
        public string ProjectDescription { get; set; }
        public IEnumerable<SubProject> SubProjects { get; set; }
    }
}
