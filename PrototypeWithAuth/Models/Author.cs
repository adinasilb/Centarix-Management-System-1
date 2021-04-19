using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PrototypeWithAuth.Models
{
    public class Author
    {
        [Key]
        public int AuthorID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
