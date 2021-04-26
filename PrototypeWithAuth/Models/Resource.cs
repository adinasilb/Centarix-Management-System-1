using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Models
{
    public class Resource
    {
        [Key]
        public int ResourceID { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string FirstAuthor { get; set; }
        public string LastAuthor { get; set; }
        public string PubMedID { get; set; }
        public string City { get; set; }
        public string Journal { get; set; }

        //public IEnumerable<TagArticle> TagArticles { get; set; }
        public int ResourceTypeID { get; set; }
        public ResourceType ResourceType { get; set; }
        public IEnumerable<ResourceResourceCategory> ResourceResourceCategories { get; set; }
    }
}
