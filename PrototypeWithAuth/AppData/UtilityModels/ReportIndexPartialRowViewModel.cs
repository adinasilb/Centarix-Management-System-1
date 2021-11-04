using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;

namespace PrototypeWithAuth.ViewModels
{
    public class ReportIndexPartialRowViewModel
    {
        private static ReportsIndexObject reportIndexObject;
        private List<IconColumnViewModel> iconList;
        private Report r;
        private ApplicationUser user;
        private FavoriteRequest favoriteRequest;
        private ShareRequest shareRequest;
        private string checkboxString;
        private FavoriteReport favoriteReport;

        public ReportIndexPartialRowViewModel() { }
        public ReportIndexPartialRowViewModel(AppUtility.ReportTypes reportType, Report report, ResourceCategory reportCategory, ReportsIndexObject reportIndexObject, FavoriteReport favoriteReport, List<IconColumnViewModel> iconList)
        {
            r = report;
            r.ReportCategory = reportCategory;
            this.favoriteReport = favoriteReport;
            ReportIndexPartialRowViewModel.reportIndexObject = reportIndexObject;
            this.iconList = iconList;
            this.user = user;
            //this.favoriteRequest = favoriteRequest;
            //this.shareRequest = shareRequest;
            //this.checkboxString = checkboxString;

            switch (reportType)
            {
                case AppUtility.ReportTypes.Weekly:
                    Columns = GetWeeklyColumns();
                    break;
            }

        }

        public IEnumerable<RequestIndexPartialColumnViewModel> Columns { get; set; }
        public string ButtonClasses { get; set; }
        public string ButtonText { get; set; }
        private static List<IconColumnViewModel> GetIcons(FavoriteReport favoriteReport, List<IconColumnViewModel> iconList )
        {
            var newIconList = AppUtility.DeepClone<List<IconColumnViewModel>>(iconList);
            //favorite icon
            var favIconIndex = newIconList.FindIndex(ni => ni.IconAjaxLink?.Contains("report-favorite")?? false);

            if (favIconIndex != -1 && favoriteReport != null) //check these checks
            {
                var unLikeIcon = new IconColumnViewModel(" icon-favorite-24px", "var(--protocols-color)", "report-favorite report-unlike", "Unfavorite");
                newIconList[favIconIndex] = unLikeIcon;
            }
            return newIconList;
        }
        private String GetSharedBy(Request request, ShareRequest shareRequest)
        {
            var applicationUser = shareRequest.FromApplicationUser;
            return applicationUser.FirstName + " " + applicationUser.LastName;
        }

        private IEnumerable<RequestIndexPartialColumnViewModel> GetWeeklyColumns()
        {
            yield return new RequestIndexPartialColumnViewModel() { Title = "Name", Width = 10, ValueWithError = new List<StringWithBool>() { new StringWithBool { String = r.ReportTitle } }, AjaxLink = "edit-report", AjaxID = r.ReportID };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Dates", Width = 10, ValueWithError = new List<StringWithBool>() { new StringWithBool { String = AppUtility.GetWeekStartEndDates(r.DateCreated) } } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "", Width = 5, Icons = GetIcons(favoriteReport, iconList), AjaxID = r.ReportID };
        }
        

    }
}
