using Microsoft.AspNetCore.Identity;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.CRUD
{
    public class ApplicationDbContextProc<T>
    {
        protected readonly ApplicationDbContext _context;
        protected readonly UserManager<ApplicationUser> _userManager;

        protected CategoryTypesProc _categoryTypesProc;
        protected CountriesProc _countriesProc;
        protected EmployeeHoursProc _employeeHoursProc;
        protected EmployeesProc _employeesProc;
        protected TimekeeperNotificationsProc _timekeeperNotificationsProc;
        protected VendorCommentsProc _vendorCommentsProc;
        protected VendorContactsProc _vendorContactsProc;
        protected VendorsProc _vendorsProc;
        protected VendorCategoryTypesProc _vendorCategoryTypesProc;
        protected EmployeeHoursAwaitingApprovalProc _employeeHoursAwaitingApprovalProc;
        protected EmployeeHoursStatuesProc _employeeHoursStatuesProc;
        protected CompanyDaysOffProc _companyDaysOffProc;
        protected OffDayTypesProc _offDayTypesProc;
        protected CommentTypesProc _commentTypesProc;
        protected RequestsProc _requestsProc;

        public ApplicationDbContextProc(ApplicationDbContext context)
        {
            _context = context;
        }

        public void InstantiateProcs()
        {
            _categoryTypesProc = new CategoryTypesProc(_context, true);
            _countriesProc = new CountriesProc(_context, true);
            _employeeHoursProc = new EmployeeHoursProc(_context, true);
            _employeesProc = new EmployeesProc(_context, true);
            _timekeeperNotificationsProc = new TimekeeperNotificationsProc(_context, true); 
            _vendorCommentsProc = new VendorCommentsProc(_context, true); 
            _vendorContactsProc = new VendorContactsProc(_context, true); 
            _vendorsProc = new VendorsProc(_context, true);
            _vendorCategoryTypesProc = new VendorCategoryTypesProc(_context, true); 
            _employeeHoursAwaitingApprovalProc = new EmployeeHoursAwaitingApprovalProc(_context, true); 
            _employeeHoursStatuesProc = new EmployeeHoursStatuesProc(_context, true); 
            _companyDaysOffProc = new CompanyDaysOffProc(_context, true); 
            _offDayTypesProc = new OffDayTypesProc(_context, true);
            _commentTypesProc = new CommentTypesProc(_context, true);
            _requestsProc = new RequestsProc(_context, true);
        }
    }
}
