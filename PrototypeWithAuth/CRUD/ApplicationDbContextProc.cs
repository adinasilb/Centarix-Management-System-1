using Microsoft.AspNetCore.Identity;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using PrototypeWithAuth.AppData;

namespace PrototypeWithAuth.CRUD
{
    public abstract class ApplicationDbContextProc<T> where T: class , ModelBase
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
        protected ParticipantsProc _participantsProc;
        protected GendersProc _gendersProc;
        protected ParticipantStatusesProc _participantStatusesProc;
        protected ExperimentEntriesProc _experimentEntriesProc;

        public ApplicationDbContextProc(ApplicationDbContext context)
        {
            _context = context;
        }

        protected void InstantiateProcs()
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
            _participantsProc = new ParticipantsProc(_context, true);
            _gendersProc = new GendersProc(_context, true);
            _participantStatusesProc = new ParticipantStatusesProc(_context, true);
            _experimentEntriesProc = new ExperimentEntriesProc(_context, true);
        }

        //public IQueryable<T> Read()
        //{

        //}


        public virtual IQueryable<T> Read(List<Expression<Func<T, bool>>> wheres = null, List<ComplexIncludes<T, ModelBase>> includes = null)
        {
            var dbset = _context.Set<T>().AsQueryable();
            if (wheres != null)
            {
                foreach (var t in wheres)
                {
                    dbset = dbset.Where(t);
                }
            }
            IIncludableQueryable<T, ModelBase> requestsWithInclude = null;
            if (includes != null)
            {
                foreach (var t in includes)
                {
                    requestsWithInclude = dbset.Include(t.Include);
                    if (t.ThenInclude !=null)
                    {
                        RecursiveInclude(requestsWithInclude, t.ThenInclude);
                    }

                }
                return requestsWithInclude.AsNoTracking().AsQueryable();
            }
            return dbset.AsNoTracking().AsQueryable();
        }

        public virtual async Task<T> ReadOne(List<Expression<Func<T, bool>>> wheres = null, List<ComplexIncludes<T, ModelBase>> includes = null)
        {
            var dbset = _context.Set<T>().AsQueryable();
            if (wheres != null)
            {
                foreach (var t in wheres)
                {
                    dbset = dbset.Where(t);
                }
            }
            var item = dbset.Take(1);
            IIncludableQueryable<T, ModelBase> requestWithInclude = null;
            if (includes != null)
            {
                foreach (var t in includes)
                {
                    requestWithInclude = item.Include(t.Include);
                    if (t.ThenInclude !=null)
                    {
                        RecursiveInclude(requestWithInclude, t.ThenInclude);
                    }

                }
            }
            return await requestWithInclude.AsNoTracking().FirstOrDefaultAsync();
        }

        public virtual IQueryable<T> ReadWithIgnoreQueryFilters(List<Expression<Func<T, bool>>> wheres = null, List<ComplexIncludes<T, ModelBase>> includes = null)
        {
            return Read(wheres, includes).IgnoreQueryFilters();
        }
        private IIncludableQueryable<ModelBase, ModelBase> RecursiveInclude(IIncludableQueryable<ModelBase, ModelBase> ObjectQueryable, ComplexIncludes<ModelBase, ModelBase> currentInclude)
        {
            ObjectQueryable = ObjectQueryable.ThenInclude(currentInclude.Include);
            if (currentInclude.ThenInclude != null)
            {
                ObjectQueryable = RecursiveInclude(ObjectQueryable, currentInclude.ThenInclude);
            }
            return ObjectQueryable;
        }

        public virtual async Task<StringWithBool> UpdateWithSaveChangesAsync(T item)
        {
            StringWithBool ReturnVal = new StringWithBool();
            try
            {
                Update(item);
                await _context.SaveChangesAsync();
                ReturnVal.SetStringAndBool(true, null);
            }
            catch (Exception ex)
            {
                ReturnVal.SetStringAndBool(false, AppUtility.GetExceptionMessage(ex));
            }
            return ReturnVal;
        }

        public virtual StringWithBool Update(T item)
        {
            StringWithBool ReturnVal = new StringWithBool();
            try
            {
                _context.Entry(item).State = EntityState.Modified;
                ReturnVal.SetStringAndBool(true, null);
            }
            catch (Exception ex)
            {
                ReturnVal.SetStringAndBool(false, AppUtility.GetExceptionMessage(ex));
            }
            return ReturnVal;
        }

        public virtual async Task<StringWithBool> CreateWithSaveChangesAsync(T item)
        {
            StringWithBool ReturnVal = new StringWithBool();
            try
            {
                Create(item);
                await _context.SaveChangesAsync();
                ReturnVal.SetStringAndBool(true, null);
            }
            catch (Exception ex)
            {
                ReturnVal.SetStringAndBool(false, AppUtility.GetExceptionMessage(ex));
            }
            return ReturnVal;
        }

        public virtual StringWithBool Create(T item)
        {
            StringWithBool ReturnVal = new StringWithBool();
            try
            {
                _context.Entry(item).State = EntityState.Added;
                ReturnVal.SetStringAndBool(true, null);
            }
            catch (Exception ex)
            {
                ReturnVal.SetStringAndBool(false, AppUtility.GetExceptionMessage(ex));
            }
            return ReturnVal;
        }

        
       
    }
}
