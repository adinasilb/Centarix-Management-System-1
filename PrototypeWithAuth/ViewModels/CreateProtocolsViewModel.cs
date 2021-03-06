using PrototypeWithAuth.AppData;
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
        public ProtocolVersion ProtocolVersion { get; set; }
        public ProtocolInstance ProtocolInstance { get; set;}
        public List<String> Tags { get; set; }
        public IEnumerable<ProtocolCategory> ProtocolCategories { get; set; }
        public IEnumerable<ProtocolSubCategory> ProtocolSubCategories { get; set; }

        public List<DocumentFolder> DocumentsInfo { get; set; }
        public IEnumerable<MaterialCategory> MaterialCategories { get; set; }
        public Lookup<Material, List<DocumentFolder>> MaterialDocuments { get; set; }
        public List<LineType> LineTypes { get; set; }
        public ProtocolsLinesViewModel Lines { get; set; }
        public IEnumerable<FunctionType> ProtocolFunctionTypes { get; set; }
        public IEnumerable<FunctionType> ResultsFunctionTypes { get; set; }
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
        public AppUtility.ProtocolModalType ModalType { get; set; }
        public Guid UniqueGuid { get; set; }
        public List<string> LastUrls { get; set; }

    }
}
