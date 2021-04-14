using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class TagArticle
    {

        [Key, Column(Order = 1)]
        public int ArticleID { get; set; }
        public Article Article { get; set; }
        [Key, Column(Order = 2)]
        public int TagID { get; set; }
        public Tag Tag { get; set; }
    }
}
