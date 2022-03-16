using PrototypeWithAuth.AppData.UtilityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.Models;
using PrototypeWithAuth.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using PrototypeWithAuth.AppData;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using Newtonsoft.Json;


namespace PrototypeWithAuth.ViewModels

{
    public class UploadDocumentInfoViewModel : ViewModelBase
    {
        public string ObjectID { get; set; }
        public DateTime Date { get; set; }
        public string FolderName { get; set; }
        public string Number { get; set; }
        public List<IFormFile> FilesToSave { get; set; }
        
    }
}
