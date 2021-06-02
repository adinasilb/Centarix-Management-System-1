using Microsoft.AspNetCore.Http;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class AddResourceViewModel
    {
        //public int ResourceType { get; set; }
        public Resource Resource { get; set; }
        [Display(Name = "Resource Image")]
        public IFormFile ResourceImage { get; set; }
        public string ResourceImageSaved { get; set; }
        public List<ResourceCategory> ResourceCategories { get; set; }
        public ResourceCategoriesToAdd[] ResourceCategoriesToAdd { get; set; }
    }
}
