﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrototypeWithAuth.AppData;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using PrototypeWithAuth.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.Controllers
{
    public class ControllerBase : Controller
    {
        private readonly ApplicationDbContext _context;

        public ControllerBase(ApplicationDbContext context)
        {
            _context = context;
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
        public SummaryHoursViewModel SummaryHoursFunction(int month, int year, Employee user)
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
            return summaryHoursViewModel;
        }


        public JsonResult GetRequestJson(Request request)
        {
            return Json(request);
        }
    }
}
