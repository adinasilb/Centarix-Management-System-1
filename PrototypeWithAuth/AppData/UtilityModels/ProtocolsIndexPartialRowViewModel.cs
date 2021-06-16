using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;

namespace PrototypeWithAuth.ViewModels
{
    public class ProtocolsIndexPartialRowViewModel
    {
        private static ProtocolsIndexObject protocolsIndexObject;
        private List<IconColumnViewModel> iconList;
        private Protocol p;
        private ProtocolType protocolType;
        private ProtocolSubCategory protocolSubCategory;
        private ProtocolsIndexObject protocolsIndexObject1;

        public ProtocolsIndexPartialRowViewModel() { }
        public ProtocolsIndexPartialRowViewModel( Protocol protocol, ProtocolType protocolType, ProtocolSubCategory protocolSubCategory, ApplicationUser user,  ProtocolsIndexObject protocolsIndexObject)
        {
            p = protocol;
            ProtocolsIndexPartialRowViewModel.protocolsIndexObject = protocolsIndexObject;
            p.ProtocolSubCategory = protocolSubCategory;
            p.ApplicationUserCreator = user;
            p.ProtocolType = protocolType;
            switch (protocolsIndexObject.SidebarType)
            {
                case AppUtility.SidebarEnum.List:
                    Columns = GetProtocolsList();
                    break;
                case AppUtility.SidebarEnum.MyProtocols:
                    Columns = GetMyProtocols();
                    break;
                case AppUtility.SidebarEnum.Favorites:
                    break;
                case AppUtility.SidebarEnum.SharedWithMe:
                    break;
            }

        }

        public ProtocolsIndexPartialRowViewModel(Protocol p, ProtocolType protocolType, ProtocolSubCategory protocolSubCategory, ProtocolsIndexObject protocolsIndexObject): this(p, protocolType, protocolSubCategory, null, protocolsIndexObject)
        {
        }

        public IEnumerable<RequestIndexPartialColumnViewModel> Columns { get; set; }
        public string ButtonClasses { get; set; }
        public string ButtonText { get; set; }

        private IEnumerable<RequestIndexPartialColumnViewModel> GetProtocolsList()
        {
            yield return new RequestIndexPartialColumnViewModel() { Title = "", Width = 10, Image = "" };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Name", AjaxLink = " load-protocol ", AjaxID = p.ProtocolID, Width = 15, Value = new List<string>() { p.Name } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Version", Width = 10, Value = new List<string>() { p.VersionNumber } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Creator", Width = 10, Value = new List<string>() { p.ApplicationUserCreator.FirstName + " " + p.ApplicationUserCreator.LastName } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Time", Width = 11, Value = new List<string>() { } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Date Created", Width = 12, Value = new List<string>() { p.CreationDate.ToString("dd'/'MM'/'yyyy") } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Type", Width = 10, Value = new List<string>() { p.ProtocolType.ProtocolTypeDescription } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Category", Width = 12, Value = new List<string>() { p.ProtocolSubCategory.ProtocolSubCategoryTypeDescription } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "", Width = 10, Icons = iconList, AjaxID = p.ProtocolID };
        }

        private IEnumerable<RequestIndexPartialColumnViewModel> GetMyProtocols()
        {
            yield return new RequestIndexPartialColumnViewModel() { Title = "", Width = 10, Image = "" };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Name", AjaxLink = " load-protocol ", AjaxID = p.ProtocolID, Width = 15, Value = new List<string>() { p.Name } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Version", Width = 10, Value = new List<string>() { p.VersionNumber } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Time", Width = 11, Value = new List<string>() { } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Date Created", Width = 12, Value = new List<string>() { p.CreationDate.ToString("dd'/'MM'/'yyyy") } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Type", Width = 10, Value = new List<string>() { p.ProtocolType.ProtocolTypeDescription } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Category", Width = 12, Value = new List<string>() { p.ProtocolSubCategory.ProtocolSubCategoryTypeDescription } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "", Width = 10, Icons = iconList, AjaxID = p.ProtocolID };
        }

    }
}
