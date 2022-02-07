using PrototypeWithAuth.AppData;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Models;

namespace PrototypeWithAuth.ViewModels
{
    public class DocumentsCardViewModel : ViewModelBase
    {
        public DocumentFolder DocumentInfo { get; set; }
        public AppUtility.MenuItems SectionType { get; set; }
        public AppUtility.RequestModalType ModalType { get; set; }
        public bool ShowSwitch { get; set; }
    }
}
