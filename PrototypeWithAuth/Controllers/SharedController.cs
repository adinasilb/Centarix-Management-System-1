using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using PrototypeWithAuth.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Controllers
{
    public class SharedController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;
        public SharedController(ApplicationDbContext context, IHostingEnvironment hostingEnvironment =null)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        private List<EmployeeHoursAndAwaitingApprovalViewModel> GetHours(DateTime monthDate, Employee user)
        {
            var hours = _context.EmployeeHours.Include(eh => eh.OffDayType).Include(eh => eh.EmployeeHoursStatusEntry1).Include(eh => eh.CompanyDayOff).ThenInclude(cdo => cdo.CompanyDayOffType).Where(eh => eh.EmployeeID == user.Id).Where(eh => eh.Date.Month == monthDate.Month && eh.Date.Year == monthDate.Year && eh.Date.Date <= DateTime.Now.Date)
                .OrderByDescending(eh => eh.Date).ToList();

            List<EmployeeHoursAndAwaitingApprovalViewModel> hoursList = new List<EmployeeHoursAndAwaitingApprovalViewModel>();
            foreach (var hour in hours)
            {
                var ehaaavm = new EmployeeHoursAndAwaitingApprovalViewModel()
                {
                    EmployeeHours = hour
                };
                var eha = _context.EmployeeHoursAwaitingApprovals.Where(ehaa => ehaa.EmployeeHoursID == hour.EmployeeHoursID).FirstOrDefault();
                if (eha!=null)
                {
                    ehaaavm.EmployeeHoursAwaitingApproval = eha;
                }
                hoursList.Add(ehaaavm);
            }
            return hoursList;
        }
        public SummaryHoursViewModel SummaryHoursFunction(int month, int year, Employee user, string errorMessage = null)
        {
            var hours = GetHours(new DateTime(year, month, DateTime.Now.Day), user);
            var CurMonth = new DateTime(year, month, DateTime.Now.Day);
            double? totalhours;
            double vacationDaysTaken = 0;
            if (user.EmployeeStatusID != 1)
            {
                totalhours = null;
            }
            else
            {
                vacationDaysTaken = _context.EmployeeHours.Where(eh => eh.EmployeeID == user.Id && eh.Date.Year == year && eh.OffDayTypeID == 2 && eh.Date <= DateTime.Now.Date && eh.Date.Month == month).Count();
                var sickCount = _context.EmployeeHours.Where(eh => eh.Date.Month == month && eh.Date.Year == year &&  eh.OffDayTypeID == 1 && eh.Date <= DateTime.Now.Date).Count();
                var vacationHours = _context.EmployeeHours.Where(eh => eh.EmployeeID == user.Id && eh.Date.Year == year && eh.PartialOffDayTypeID == 2 && eh.Date <= DateTime.Now.Date && eh.Date.Month == month).Select(eh => (eh.PartialOffDayHours == null ? TimeSpan.Zero : ((TimeSpan)eh.PartialOffDayHours)).TotalHours).ToList().Sum(p => p);
                vacationDaysTaken = Math.Round(vacationDaysTaken + (vacationHours / user.SalariedEmployee.HoursPerDay), 2);
                totalhours = AppUtility.GetTotalWorkingDaysThisMonth(new DateTime(year, month, 1), _context.CompanyDayOffs, vacationDaysTaken+sickCount) * user.SalariedEmployee.HoursPerDay;
               
            }
            SummaryHoursViewModel summaryHoursViewModel = new SummaryHoursViewModel()
            {
                EmployeeHours = hours,
                CurrentMonth = CurMonth,
                TotalHoursInMonth = totalhours,
                SelectedYear = year,
                TotalHolidaysInMonth = _context.CompanyDayOffs.Where(cdo => cdo.Date.Year == year && cdo.Date.Month == month).Count(),
                VacationDayInThisMonth = vacationDaysTaken,
                User = user
            };
            if(errorMessage != null)
            {
                summaryHoursViewModel.ErrorMessage += errorMessage;
            }
            return summaryHoursViewModel;
        }

        public double GetUsersOffDaysLeft(Employee user, int offDayTypeID, int thisYear)
        {
            var year = AppUtility.YearStartedTimeKeeper;
            var offDaysLeft = 0.0;
            while (year <= thisYear)
            {
                double vacationDays = 0;
                double sickDays = 0;
                double offDaysTaken = _context.EmployeeHours.Where(eh => eh.EmployeeID == user.Id && eh.Date.Year == year && eh.OffDayTypeID == offDayTypeID && eh.Date <= DateTime.Now.Date &&eh.IsBonus==false).Count();
                if (user.EmployeeStatusID == 1 && offDayTypeID==2)
                {
                    var vacationHours = _context.EmployeeHours.Where(eh => eh.EmployeeID == user.Id && eh.Date.Year == year && eh.PartialOffDayTypeID == 2 && eh.Date <= DateTime.Now.Date && eh.IsBonus == false).Select(eh => (eh.PartialOffDayHours == null ? TimeSpan.Zero : ((TimeSpan)eh.PartialOffDayHours)).TotalHours).ToList().Sum(p => p);
                    offDaysTaken = Math.Round(offDaysTaken + (vacationHours / user.SalariedEmployee.HoursPerDay), 2);
                }
                if (year == AppUtility.YearStartedTimeKeeper && year == DateTime.Now.Year)
                {
                    int month = DateTime.Now.Month;
                    vacationDays = (user.VacationDaysPerMonth * month) + user.RollOverVacationDays;
                    sickDays = (user.SickDaysPerMonth * month) + user.RollOverSickDays;
                }
                else if (year == AppUtility.YearStartedTimeKeeper)
                {
                    int month = DateTime.Now.Month;
                    vacationDays = user.VacationDays + user.RollOverVacationDays;
                    sickDays = user.SickDays + user.RollOverSickDays;
                }
                else if (year == user.StartedWorking.Year)
                {
                    int month = 12 - user.StartedWorking.Month + 1;
                    vacationDays = user.VacationDaysPerMonth * month;
                    sickDays = user.SickDays * month;
                }
                else if (year == DateTime.Now.Year)
                {
                    int month = DateTime.Now.Month;
                    vacationDays = (user.VacationDaysPerMonth * month);
                    sickDays = (user.SickDaysPerMonth * month);
                }
                else
                {
                    vacationDays = user.VacationDays;
                    sickDays = user.SickDays;
                }
                if (offDayTypeID == 2)
                {
                    offDaysLeft += vacationDays-offDaysTaken;
                }
                else
                {
                    offDaysLeft += sickDays - offDaysTaken;
                }               
                year = year + 1;
            }
            return offDaysLeft;
        }


        public void RemoveRequestWithCommentsAndEmailSessions()
        {
            var requiredKeys = HttpContext.Session.Keys.Where(x => x.StartsWith(AppData.SessionExtensions.SessionNames.Request.ToString()) ||
                x.StartsWith(AppData.SessionExtensions.SessionNames.Comment.ToString()) ||
                 x.StartsWith(AppData.SessionExtensions.SessionNames.Email.ToString()));
            foreach (var k in requiredKeys)
            {
                HttpContext.Session.Remove(k); //will clear the session for the future
            }

        }

        public decimal GetExchangeRateIfNull()
        {
            return _context.ExchangeRates.Select(er => er.LatestExchangeRate).FirstOrDefault();
        }


        public void GetExistingFileStrings(List<DocumentFolder> DocumentsInfo, AppUtility.FolderNamesEnum folderName, string uploadFolderParent)
        {
            string uploadFolder = Path.Combine(uploadFolderParent, folderName.ToString());
            DocumentFolder folder = new DocumentFolder()
            {
                FolderName = folderName
            };
            if (Directory.Exists(uploadFolder))
            {
                DirectoryInfo DirectoryToSearch = new DirectoryInfo(uploadFolder);
                //searching for the partial file name in the directory
                FileInfo[] orderfilesfound = DirectoryToSearch.GetFiles("*.*");
                folder.FileStrings = new List<string>();
                foreach (var orderfile in orderfilesfound)
                {
                    string newFileString = AppUtility.GetLastFiles(orderfile.FullName, 4);
                    folder.FileStrings.Add(newFileString);
                }
            }
            folder.Icon = AppUtility.GetDocumentIcon(folderName);

            DocumentsInfo.Add(folder);
        }

        public virtual void DocumentsModal(DocumentsModalViewModel documentsModalViewModel)
        {
            string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, documentsModalViewModel.ParentFolderName.ToString());
            string folder = Path.Combine(uploadFolder, documentsModalViewModel.ObjectID.ToString());
            Directory.CreateDirectory(folder);
            if (documentsModalViewModel.FilesToSave != null) //test for more than one???
            {
                var x = 1;
                foreach (IFormFile file in documentsModalViewModel.FilesToSave)
                {
                    //create file
                    string folderPath = Path.Combine(folder, documentsModalViewModel.FolderName.ToString());
                    Directory.CreateDirectory(folderPath);
                    string uniqueFileName = x + file.FileName;
                    string filePath = Path.Combine(folderPath, uniqueFileName);
                    FileStream filestream = new FileStream(filePath, FileMode.Create);
                    file.CopyTo(filestream);
                    filestream.Close();
                    x++;
                }
            }
        }
    }
}
