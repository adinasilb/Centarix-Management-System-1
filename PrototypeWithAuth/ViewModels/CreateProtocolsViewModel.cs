using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.ViewModels
{
    public class CreateProtocolsViewModel : ViewModelBase
    {
        public Protocol Protocol { get; set; }
        public List<String> Tags { get; set; }
        public IEnumerable<ProtocolCategory> ProtocolCategories { get; set; }
        public IEnumerable<ProtocolSubCategory> ProtocolSubCategories { get; set; }

        public List<DocumentFolder> DocumentsInfo { get; set; }
        public IEnumerable<MaterialCategory> MaterialCategories { get; set; }
        public Lookup<Material, List<DocumentFolder>> MaterialDocuments { get; set; }
        public List<LineType> LineTypes { get; set; }
        public IEnumerable<ProtocolsLineViewModel> TempLines { get; set; }
        public IEnumerable<FunctionType> FunctionTypes { get; set; }
        private int _Tab;
        public int Tab
        {
            get
            {
                if (_Tab == 0) { return 1; }
                else
                {
                    return _Tab;
                }
            }
            set { _Tab = value; }
        }
    }
}
