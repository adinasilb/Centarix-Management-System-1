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
    public class ReportIndexPartialRowViewModel
    {
        private static ReportsIndexObject reportIndexObject;
        private List<IconColumnViewModel> iconList;
        private Report r;
        private ApplicationUser user;
        private FavoriteRequest favoriteRequest;
        private ShareRequest shareRequest;
        private string checkboxString;

        public ReportIndexPartialRowViewModel() { }
        public ReportIndexPartialRowViewModel(AppUtility.ReportTypes reportType, Report report, ResourceCategory reportCategory, ReportsIndexObject reportIndexObject)
        {
            r = report;
            r.ReportCategory = reportCategory;
            ReportIndexPartialRowViewModel.reportIndexObject = reportIndexObject;
            //this.iconList = iconList;
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
        private static List<IconColumnViewModel> GetIcons()
        {
            var newIconList = new List<IconColumnViewModel>();
            //var editIcon = new IconColumnViewModel("icon-edit_doc-24px", "#009688", "edit-report");
            //newIconList.Add(editIcon);
            return newIconList;
        }
        private String GetSharedBy(Request request, ShareRequest shareRequest)
        {
            var applicationUser = shareRequest.FromApplicationUser;
            return applicationUser.FirstName + " " + applicationUser.LastName;
        }

        private IEnumerable<RequestIndexPartialColumnViewModel> GetWeeklyColumns()
        {
            yield return new RequestIndexPartialColumnViewModel() { Title = "Name", Width = 10, Value = new List<string>() { r.ReportTitle }, AjaxLink = "edit-report", AjaxID = r.ReportID };
            yield return new RequestIndexPartialColumnViewModel() { Title = "Dates", Width = 10, Value = new List<string>() { AppUtility.GetWeekStartEndDates(r.DateCreated) } };
            yield return new RequestIndexPartialColumnViewModel() { Title = "", Width = 5, Icons = GetIcons(), AjaxID = r.ReportID };
        }
        

    }
}
