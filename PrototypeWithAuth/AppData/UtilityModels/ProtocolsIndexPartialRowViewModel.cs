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
        private ProtocolVersion pv;
        private ProtocolType protocolType;
        private ProtocolSubCategory protocolSubCategory;
        private static string defaultImage;
        private ApplicationUser user;
        private FavoriteProtocol favoriteProtocol;
        private ShareProtocol shareProtocol;
        private ProtocolInstance protocolInstance;

        public ProtocolsIndexPartialRowViewModel() { }

        public ProtocolsIndexPartialRowViewModel(ProtocolVersion pv, Protocol protocol, ProtocolType protocolType, ProtocolSubCategory protocolSubCategory, ApplicationUser userCreator,  ProtocolsIndexObject protocolsIndexObject, List<IconColumnViewModel> iconList, FavoriteProtocol favoriteProtocol, ApplicationUser user, ProtocolInstance protocolInstance) : this(pv, protocol, protocolType, protocolSubCategory, userCreator, protocolsIndexObject, iconList,  null, favoriteProtocol, user, protocolInstance)
        {         
        }
        public ProtocolsIndexPartialRowViewModel(ProtocolVersion pv, Protocol protocol, ProtocolType protocolType, ProtocolSubCategory protocolSubCategory, ApplicationUser userCreator, ProtocolsIndexObject protocolsIndexObject, List<IconColumnViewModel> iconList, ApplicationUser user, ProtocolInstance protocolInstance) : this(pv, protocol, protocolType, protocolSubCategory, userCreator, protocolsIndexObject, iconList, null, user, protocolInstance)
        {
        }
        public ProtocolsIndexPartialRowViewModel(ProtocolVersion pv, Protocol protocol, ProtocolType protocolType, ProtocolSubCategory protocolSubCategory, ProtocolsIndexObject protocolsIndexObject, List<IconColumnViewModel> iconList, FavoriteProtocol favoriteProtocol, ApplicationUser user, ProtocolInstance protocolInstance) : this(pv, protocol, protocolType, protocolSubCategory, null, protocolsIndexObject, iconList, favoriteProtocol, user, protocolInstance)
        {
        }
        public ProtocolsIndexPartialRowViewModel(ProtocolVersion pv, Protocol protocol, ProtocolType protocolType, ProtocolSubCategory protocolSubCategory, ProtocolsIndexObject protocolsIndexObject, List<IconColumnViewModel> iconList, ApplicationUser userCreator, ShareProtocol shareProtocol, ApplicationUser user, ProtocolInstance protocolInstance) : this(pv, protocol, protocolType, protocolSubCategory, userCreator, protocolsIndexObject, iconList, shareProtocol, null, user, protocolInstance)
        {
        }
        public ProtocolsIndexPartialRowViewModel(ProtocolVersion pv, Protocol protocol, ProtocolType protocolType, ProtocolSubCategory protocolSubCategory, ApplicationUser userCreator, ProtocolsIndexObject protocolsIndexObject, List<IconColumnViewModel> iconList, ShareProtocol shareProtocol, FavoriteProtocol favoriteProtocol, ApplicationUser user, ProtocolInstance protocolInstance)
        {
            this.pv = pv;
            this.pv.Protocol = protocol;
            ProtocolsIndexPartialRowViewModel.protocolsIndexObject = protocolsIndexObject;
            this.pv.Protocol.ProtocolSubCategory = protocolSubCategory;
            this.pv.ApplicationUserCreator = userCreator;
            this.user = user;
            ProtocolsIndexPartialRowViewModel.iconList = iconList;
            this.pv.Protocol.ProtocolType = protocolType;
            this.favoriteProtocol = favoriteProtocol;
            this.shareProtocol = shareProtocol;
            this.protocolInstance = protocolInstance;

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
                case AppUtility.SidebarEnum.LastProtocol:
                    Columns = GetLastProtocolList();
                    break;
            }
        }
        
        public IEnumerable<RequestIndexPartialColumnViewModel> Columns { get; set; }
        public string ButtonClasses { get; set; }
        public string ButtonText { get; set; }

        private IEnumerable<RequestIndexPartialColumnViewModel> GetProtocolsList()
        {
            yield return new RequestIndexPartialColumnViewModel() { Title = "", Width = 10, Image = "" };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Name", AjaxLink = " load-protocol ", AjaxID = pv.ProtocolVersionID, Width = 15, Value = new List<string>() { pv.Protocol.Name } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Version", Width = 10, Value = new List<string>() { pv.VersionNumber+"" } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Creator", Width = 10, Value = new List<string>() { pv.ApplicationUserCreator.FirstName + " " + pv.ApplicationUserCreator.LastName } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Time", Width = 11, Value = new List<string>() { } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Date Created", Width = 12, Value = new List<string>() { pv.CreationDate.ToString("dd'/'MM'/'yyyy") } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Type", Width = 10, Value = new List<string>() { pv.Protocol.ProtocolType.ProtocolTypeDescription } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Category", Width = 12, Value = new List<string>() { pv.Protocol.ProtocolSubCategory.ProtocolSubCategoryTypeDescription } };
            yield return new RequestIndexPartialColumnViewModel()
            {
                Title = "",
                Width = 10,
                Icons = GetIconsByIndividualProtocol(favoriteProtocol, user),
                AjaxID = pv.ProtocolVersionID
            };  }
        private IEnumerable<RequestIndexPartialColumnViewModel> GetLastProtocolList()
        {
            yield return new RequestIndexPartialColumnViewModel() { Title = "", Width = 10, Image = "" };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Name", AjaxLink = " load-protocol ", AjaxID = pv.ProtocolVersionID, Width = 15, Value = new List<string>() { pv.Protocol.Name } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Version", Width = 8, Value = new List<string>() { pv.VersionNumber+"" } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Creator", Width = 10, Value = new List<string>() { pv.ApplicationUserCreator.FirstName + " " + pv.ApplicationUserCreator.LastName } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Time", Width = 8, Value = new List<string>() { } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Start Date", Width = 10, Value = new List<string>() { protocolInstance.StartDate.GetElixirDateFormatWithTime() } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "End Date", Width = 10, Value = new List<string>() { protocolInstance.EndDate.GetElixirDateFormatWithTime()} };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Category", Width = 12, Value = new List<string>() { pv.Protocol.ProtocolSubCategory.ProtocolSubCategoryTypeDescription } };
            yield return new RequestIndexPartialColumnViewModel()
            {
                Title = "",
                Width = 10,
                Icons = iconList,
                AjaxID =protocolInstance.ProtocolInstanceID
            };
        }
        private IEnumerable<RequestIndexPartialColumnViewModel> GetMyProtocols()
        {
            yield return new RequestIndexPartialColumnViewModel() { Title = "", Width = 10, Image = "" };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Name", AjaxLink = " load-protocol ", AjaxID = pv.ProtocolVersionID, Width = 15, Value = new List<string>() { pv.Protocol.Name } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Version", Width = 10, Value = new List<string>() { pv.VersionNumber+"" } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Time", Width = 11, Value = new List<string>() { } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Date Created", Width = 12, Value = new List<string>() { pv.CreationDate.GetElixirDateFormat()} };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Type", Width = 10, Value = new List<string>() { pv.Protocol.ProtocolType.ProtocolTypeDescription } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Category", Width = 12, Value = new List<string>() { pv.Protocol.ProtocolSubCategory.ProtocolSubCategoryTypeDescription } };
            yield return new RequestIndexPartialColumnViewModel()
            {
                Title = "",
                Width = 10,
                Icons = GetIconsByIndividualProtocol(favoriteProtocol, user),
                AjaxID = pv.ProtocolVersionID
            };
        }
        private IEnumerable<RequestIndexPartialColumnViewModel> GetSharedColumns()
        {
            yield return new RequestIndexPartialColumnViewModel() { Title = "", Width = 10, Image = "" };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Name", AjaxLink = " load-protocol ", AjaxID = pv.ProtocolVersionID, Width = 15, Value = new List<string>() { pv.Protocol.Name } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Version", Width = 10, Value = new List<string>() { pv.VersionNumber+"" } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Creator", Width = 10, Value = new List<string>() { pv.ApplicationUserCreator.FirstName + " " + pv.ApplicationUserCreator.LastName } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Time", Width = 11, Value = new List<string>() { } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Date Created", Width = 12, Value = new List<string>() { pv.CreationDate.GetElixirDateFormat() } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Type", Width = 10, Value = new List<string>() { pv.Protocol.ProtocolType.ProtocolTypeDescription } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Category", Width = 12, Value = new List<string>() { pv.Protocol.ProtocolSubCategory.ProtocolSubCategoryTypeDescription } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Shared By", Width = 10, Value = new List<string>() { GetSharedBy(shareProtocol) } };
            yield return new RequestIndexPartialColumnViewModel()
            {
                Title = "",
                Width = 10,
                Icons = GetIconsByIndividualProtocol(favoriteProtocol, user),
                AjaxID = pv.ProtocolVersionID
            };
        }
        private IEnumerable<RequestIndexPartialColumnViewModel> GetFavoriteColumns()
        {
            yield return new RequestIndexPartialColumnViewModel() { Title = "", Width = 10, Image = "" };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Name", AjaxLink = " load-protocol ", AjaxID = pv.ProtocolVersionID, Width = 15, Value = new List<string>() { pv.Protocol.Name } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Version", Width = 10, Value = new List<string>() { pv.VersionNumber+"" } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Creator", Width = 10, Value = new List<string>() { pv.ApplicationUserCreator.FirstName + " " + pv.ApplicationUserCreator.LastName } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Time", Width = 11, Value = new List<string>() { } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Date Created", Width = 12, Value = new List<string>() { pv.CreationDate.GetElixirDateFormat() } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Type", Width = 10, Value = new List<string>() { pv.Protocol.ProtocolType.ProtocolTypeDescription } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Category", Width = 12, Value = new List<string>() { pv.Protocol.ProtocolSubCategory.ProtocolSubCategoryTypeDescription } };
            yield return new RequestIndexPartialColumnViewModel()
            {
                Title = "",
                Width = 10,
                Icons = GetIconsByIndividualProtocol(favoriteProtocol, user),
                AjaxID = pv.ProtocolVersionID
            };
        }
        private static List<IconColumnViewModel> GetIconsByIndividualProtocol( FavoriteProtocol favoriteProtocol, ApplicationUser user)
        {
            var newIconList = AppUtility.DeepClone<List<IconColumnViewModel>>(iconList);
            //favorite icon
            var favIconIndex = newIconList.FindIndex(ni => ni.IconAjaxLink.Contains("protocol-favorite"));

            if (favIconIndex != -1 && favoriteProtocol != null) //check these checks
            {
                var unLikeIcon = new IconColumnViewModel(" icon-favorite-24px", "var(--protocols-color)", "protocol-favorite protocol-unlike", "Unfavorite");
                newIconList[favIconIndex] = unLikeIcon;
            }
            //var morePopoverIndex = newIconList.FindIndex(ni => ni.IconAjaxLink.Contains("popover-more"));
            //if (morePopoverIndex != -1)
            //{
            //    //var newMorePopoverList = AppUtility.DeepClone(newIconList[morePopoverIndex]);
            //    var startIndex = newIconList.ElementAt(morePopoverIndex).IconPopovers.FindIndex(ni => ni.Description==AppUtility.PopoverDescription.Start);
            //    if(startIndex !=-1 && protocolInstance!=null)
            //    {
            //        var continueIcon = new IconPopoverViewModel("icon-play_circle_outline-24px-1", "#4CAF50", AppUtility.PopoverDescription.Continue, "StartProtocol", "Protocols", AppUtility.PopoverEnum.None, "start-protocol-fx");
            //        newIconList[morePopoverIndex].IconPopovers[startIndex] = continueIcon;
            //    }
            //}
            return newIconList;
        }
        private String GetSharedBy( ShareProtocol shareProtocol)
        {
            var applicationUser = shareProtocol.FromApplicationUser;
            return applicationUser.FirstName + " " + applicationUser.LastName;
        }
    }
}
