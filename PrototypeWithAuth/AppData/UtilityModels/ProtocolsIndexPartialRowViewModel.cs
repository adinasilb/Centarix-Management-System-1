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
        private static List<IconColumnViewModel> iconList;
        private Protocol p;
        private ProtocolType protocolType;
        private ProtocolSubCategory protocolSubCategory;
        private static string defaultImage;
        private ApplicationUser user;
        private FavoriteProtocol favoriteProtocol;
        private ShareProtocol shareProtocol;

        public ProtocolsIndexPartialRowViewModel() { }
        public ProtocolsIndexPartialRowViewModel( Protocol protocol, ProtocolType protocolType, ProtocolSubCategory protocolSubCategory, ApplicationUser userCreator,  ProtocolsIndexObject protocolsIndexObject, List<IconColumnViewModel> iconList, FavoriteProtocol favoriteProtocol, ApplicationUser user) : this(protocol, protocolType, protocolSubCategory, userCreator, protocolsIndexObject, iconList,  null, favoriteProtocol, user)
        {         
        }

        public ProtocolsIndexPartialRowViewModel(Protocol protocol, ProtocolType protocolType, ProtocolSubCategory protocolSubCategory, ProtocolsIndexObject protocolsIndexObject, List<IconColumnViewModel> iconList, FavoriteProtocol favoriteProtocol, ApplicationUser user) : this(protocol, protocolType, protocolSubCategory, null, protocolsIndexObject, iconList, favoriteProtocol, user)
        {
        }
        public ProtocolsIndexPartialRowViewModel(Protocol protocol, ProtocolType protocolType, ProtocolSubCategory protocolSubCategory, ProtocolsIndexObject protocolsIndexObject, List<IconColumnViewModel> iconList, ApplicationUser userCreator, ShareProtocol shareProtocol, ApplicationUser user) : this(protocol, protocolType, protocolSubCategory, userCreator, protocolsIndexObject, iconList, shareProtocol, null, user)
        {
        }
        public ProtocolsIndexPartialRowViewModel(Protocol protocol, ProtocolType protocolType, ProtocolSubCategory protocolSubCategory, ApplicationUser userCreator, ProtocolsIndexObject protocolsIndexObject, List<IconColumnViewModel> iconList, ShareProtocol shareProtocol, FavoriteProtocol favoriteProtocol, ApplicationUser user)
        {
            p = protocol;
            ProtocolsIndexPartialRowViewModel.protocolsIndexObject = protocolsIndexObject;
            p.ProtocolSubCategory = protocolSubCategory;
            p.ApplicationUserCreator = userCreator;
            this.user = user;
            ProtocolsIndexPartialRowViewModel.iconList = iconList;
            p.ProtocolType = protocolType;
            this.favoriteProtocol = favoriteProtocol;
            this.shareProtocol = shareProtocol;
            switch (protocolsIndexObject.SidebarType)
            {
                case AppUtility.SidebarEnum.List:
                    Columns = GetProtocolsList();
                    break;
                case AppUtility.SidebarEnum.MyProtocols:
                    Columns = GetMyProtocols();
                    break;
                case AppUtility.SidebarEnum.Favorites:
                    Columns = GetFavoriteColumns();
                    break;
                case AppUtility.SidebarEnum.SharedWithMe:
                    Columns = GetSharedColumns();
                    break;
            }
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
             yield return new RequestIndexPartialColumnViewModel()
            {
                Title = "",
                Width = 10,
                Icons = GetIconsByIndividualProtocol(favoriteProtocol, user),
                AjaxID = p.ProtocolID
            };  }

        private IEnumerable<RequestIndexPartialColumnViewModel> GetMyProtocols()
        {
            yield return new RequestIndexPartialColumnViewModel() { Title = "", Width = 10, Image = "" };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Name", AjaxLink = " load-protocol ", AjaxID = p.ProtocolID, Width = 15, Value = new List<string>() { p.Name } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Version", Width = 10, Value = new List<string>() { p.VersionNumber } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Time", Width = 11, Value = new List<string>() { } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Date Created", Width = 12, Value = new List<string>() { p.CreationDate.ToString("dd'/'MM'/'yyyy") } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Type", Width = 10, Value = new List<string>() { p.ProtocolType.ProtocolTypeDescription } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Category", Width = 12, Value = new List<string>() { p.ProtocolSubCategory.ProtocolSubCategoryTypeDescription } };
            yield return new RequestIndexPartialColumnViewModel()
            {
                Title = "",
                Width = 10,
                Icons = GetIconsByIndividualProtocol(favoriteProtocol, user),
                AjaxID = p.ProtocolID
            };
        }
        private IEnumerable<RequestIndexPartialColumnViewModel> GetSharedColumns()
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
            yield return new RequestIndexPartialColumnViewModel() { Title = "Shared By", Width = 10, Value = new List<string>() { GetSharedBy(shareProtocol) } };
            yield return new RequestIndexPartialColumnViewModel()
            {
                Title = "",
                Width = 10,
                Icons = GetIconsByIndividualProtocol(favoriteProtocol, user),
                AjaxID = p.ProtocolID
            };
        }
        private IEnumerable<RequestIndexPartialColumnViewModel> GetFavoriteColumns()
        {
            yield return new RequestIndexPartialColumnViewModel() { Title = "", Width = 10, Image = "" };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Name", AjaxLink = " load-protocol ", AjaxID = p.ProtocolID, Width = 15, Value = new List<string>() { p.Name } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Version", Width = 10, Value = new List<string>() { p.VersionNumber } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Creator", Width = 10, Value = new List<string>() { p.ApplicationUserCreator.FirstName + " " + p.ApplicationUserCreator.LastName } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Time", Width = 11, Value = new List<string>() { } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Date Created", Width = 12, Value = new List<string>() { p.CreationDate.ToString("dd'/'MM'/'yyyy") } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Type", Width = 10, Value = new List<string>() { p.ProtocolType.ProtocolTypeDescription } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Category", Width = 12, Value = new List<string>() { p.ProtocolSubCategory.ProtocolSubCategoryTypeDescription } };
            yield return new RequestIndexPartialColumnViewModel()
            {
                Title = "",
                Width = 10,
                Icons = GetIconsByIndividualProtocol(favoriteProtocol, user),
                AjaxID = p.ProtocolID
            };
        }
        private static List<IconColumnViewModel> GetIconsByIndividualProtocol( FavoriteProtocol favoriteProtocol = null, ApplicationUser user = null)
        {
            var newIconList = AppUtility.DeepClone(iconList);
            //favorite icon
            var favIconIndex = newIconList.FindIndex(ni => ni.IconAjaxLink.Contains("protocol-favorite"));

            if (favIconIndex != -1 && favoriteProtocol != null) //check these checks
            {
                var unLikeIcon = new IconColumnViewModel(" icon-favorite-24px", "#5F79E2", "protocol-favorite protocol-unlike", "Unfavorite");
                newIconList[favIconIndex] = unLikeIcon;
            }
            return newIconList;
        }
        private String GetSharedBy( ShareProtocol shareProtocol)
        {
            var applicationUser = shareProtocol.FromApplicationUser;
            return applicationUser.FirstName + " " + applicationUser.LastName;
        }
    }
}
