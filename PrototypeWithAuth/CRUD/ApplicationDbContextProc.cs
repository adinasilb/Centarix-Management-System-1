using Microsoft.AspNetCore.Identity;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using PrototypeWithAuth.AppData;
using System.Diagnostics;

namespace PrototypeWithAuth.CRUD
{
    public abstract class ApplicationDbContextProc<T> where T : class, ModelBase
    {
        protected readonly ApplicationDbContext _context;
        //protected readonly UserManager<ApplicationUser> _userManager;

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
        protected EmployeeHoursStatusesProc _employeeHoursStatuesProc;
        protected CompanyDaysOffProc _companyDaysOffProc;
        protected OffDayTypesProc _offDayTypesProc;
        protected CommentTypesProc _commentTypesProc;
        protected RequestsProc _requestsProc;
        protected ParticipantsProc _participantsProc;
        protected GendersProc _gendersProc;
        protected ParticipantStatusesProc _participantStatusesProc;
        protected ExperimentEntriesProc _experimentEntriesProc;
        protected SitesProc _sitesProc;
        protected TestsProc _testsProc;
        protected TestValuesProc _testValuesProc;
        protected ProductsProc _productsProc;
        protected ProductSubcategoriesProc _productSubcategoriesProc;
        protected ParentCategoriesProc _parentCategoriesProc;
        protected CentarixIDsProc _centarixIDsProc;
        protected EmployeeStatusesProc _employeeStatusesProc;
        protected JobSubcategoryTypesProc _jobSubcategoryTypesProc;
        protected LocationTypesProc _locationTypesProc;
        protected LocationRoomInstancesProc _locationRoomInstancesProc;
        protected LabPartsProc _labPartsProc;
        protected LocationInstancesProc _locationInstancesProc;
        protected RequestLocationInstancesProc _requestLocationInstancesProc;
        protected RequestCommentsProc _requestCommentsProc;
        protected ProductCommentsProc _productCommentsProc;
        protected ShareRequestListsProc _shareRequestListsProc;
        protected RequestListRequestsProc _requestListRequestsProc;
        protected RequestListsProc _requestListsProc;
        protected PaymentsProc _paymentsProc;
        protected ParentRequestsProc _parentRequestsProc;
        protected ParentQuotesProc _parentQuotesProc;
        protected RequestNotificationsProc _requestNotificationsProc;
        protected EmployeeInfoNotificationsProc _employeeInfoNotificationsProc;
        protected TemporaryLocationInstancesProc _temporaryLocationInstancesProc;

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
            _employeeHoursStatuesProc = new EmployeeHoursStatusesProc(_context, true);
            _companyDaysOffProc = new CompanyDaysOffProc(_context, true);
            _offDayTypesProc = new OffDayTypesProc(_context, true);
            _commentTypesProc = new CommentTypesProc(_context, true);
            _requestsProc = new RequestsProc(_context, true);
            _participantsProc = new ParticipantsProc(_context, true);
            _gendersProc = new GendersProc(_context, true);
            _participantStatusesProc = new ParticipantStatusesProc(_context, true);
            _experimentEntriesProc = new ExperimentEntriesProc(_context, true);
            _sitesProc = new SitesProc(_context, true);
            _testsProc = new TestsProc(_context, true);
            _testValuesProc = new TestValuesProc(_context, true);
            _productsProc = new ProductsProc(_context, true);
            _productSubcategoriesProc = new ProductSubcategoriesProc(_context, true);
            _parentCategoriesProc = new ParentCategoriesProc(_context, true);
            _centarixIDsProc = new CentarixIDsProc(_context, true);
            _employeeStatusesProc = new EmployeeStatusesProc(_context, true);
            _jobSubcategoryTypesProc = new JobSubcategoryTypesProc(_context, true);
            _locationTypesProc = new LocationTypesProc(_context, true);
            _locationRoomInstancesProc = new LocationRoomInstancesProc(_context, true);
            _labPartsProc = new LabPartsProc(_context, true);
            _locationInstancesProc = new LocationInstancesProc(_context, true);
            _requestLocationInstancesProc = new RequestLocationInstancesProc(_context, true);
            _requestCommentsProc = new RequestCommentsProc(_context, true);
            _productCommentsProc = new ProductCommentsProc(_context, true);
            _shareRequestListsProc = new ShareRequestListsProc(_context, true);
            _requestListRequestsProc = new RequestListRequestsProc(_context, true);
            _requestListsProc = new RequestListsProc(_context, true);
            _paymentsProc = new PaymentsProc(_context, true);
            _parentRequestsProc = new ParentRequestsProc(_context, true);
            _parentQuotesProc = new ParentQuotesProc(_context, true);
            _requestNotificationsProc  = new RequestNotificationsProc(_context, true);
            _employeeInfoNotificationsProc = new EmployeeInfoNotificationsProc(_context, true);
            _temporaryLocationInstancesProc = new TemporaryLocationInstancesProc(_context, true);
        }

        //protected void InstantiateProcs(List<String> Types)
        //{
        //    foreach(var)
        //}

        public virtual IQueryable<T> Read(List<Expression<Func<T, bool>>> wheres = null, List<ComplexIncludes<T, ModelBase>> includes = null)
        {

            var dbset = _context.Set<T>().AsQueryable();
            dbset = ApplyWheres(wheres, dbset);
            if (includes != null && includes.Count > 0)
            {
                IIncludableQueryable<T, ModelBase> ReqsWithInclude = ApplyIncludes(includes, dbset);
                return ReqsWithInclude.AsNoTracking().AsQueryable();
            }
            return dbset.AsNoTracking().AsQueryable();
        }

        private static IQueryable<T> ApplyWheres(List<Expression<Func<T, bool>>> wheres, IQueryable<T> dbset)
        {
            if (wheres != null)
            {
                foreach (var t in wheres)
                {
                    dbset = dbset.Where(t);
                }
            }
            return dbset;
        }

        private IIncludableQueryable<T, ModelBase> ApplyIncludes(List<ComplexIncludes<T, ModelBase>> includes, IQueryable<T> dbset)
        {
            IIncludableQueryable<T, ModelBase> ReqsWithInclude = dbset.Include(includes.FirstOrDefault().Include);
            for (int t = 0; t < includes.Count; t++)
            {
                ReqsWithInclude = ReqsWithInclude.Include(includes[t].Include);
                if (includes[t].ThenInclude != null)
                {
                    ReqsWithInclude = RecursiveInclude(ReqsWithInclude, includes[t].ThenInclude);
                }
            }
            return ReqsWithInclude;
        }

        public async Task<T> ReadOneAsync(List<Expression<Func<T, bool>>> wheres = null, List<ComplexIncludes<T, ModelBase>> includes = null)
        {       
            var dbset = _context.Set<T>().AsQueryable();
            dbset = ApplyWheres(wheres, dbset);
            var item = dbset.Take(1);
            if (includes != null && includes.Count > 0)
            {
                IIncludableQueryable<T, ModelBase> requestWithInclude = ApplyIncludes(includes, dbset);
                return await requestWithInclude.AsNoTracking().FirstOrDefaultAsync();
            }
            return await item.AsNoTracking().FirstOrDefaultAsync();
        }
        public virtual IQueryable<T> ReadOne(List<Expression<Func<T, bool>>> wheres = null, List<ComplexIncludes<T, ModelBase>> includes = null)
        {
            var dbset = _context.Set<T>().AsQueryable();
            dbset = ApplyWheres(wheres, dbset);
            var item = dbset.Take(1);
            if (includes != null && includes.Count > 0)
            {
                IIncludableQueryable<T, ModelBase> requestWithInclude = ApplyIncludes(includes, dbset);
                return requestWithInclude.AsNoTracking();
            }
            return item.AsNoTracking();
        }
        public virtual async Task<T> ReadOneWithIgnoreQueryFiltersAsync(List<Expression<Func<T, bool>>> wheres = null, List<ComplexIncludes<T, ModelBase>> includes = null)
        {
            return await ReadOne(wheres, includes).IgnoreQueryFilters().FirstOrDefaultAsync();
        }

        public virtual IQueryable<T> ReadOneWithIgnoreQueryFilters(List<Expression<Func<T, bool>>> wheres = null, List<ComplexIncludes<T, ModelBase>> includes = null)
        {
            return ReadOne(wheres, includes).IgnoreQueryFilters();
        }

        public virtual IQueryable<T> ReadWithIgnoreQueryFilters(List<Expression<Func<T, bool>>> wheres = null, List<ComplexIncludes<T, ModelBase>> includes = null)
        {
            return Read(wheres, includes).IgnoreQueryFilters();
        }

        private IIncludableQueryable<T, ModelBase> RecursiveInclude(IIncludableQueryable<T, ModelBase> ObjectQueryable, ComplexIncludes<ModelBase, ModelBase> currentInclude)
        {
            ObjectQueryable = ObjectQueryable.ThenInclude(currentInclude.Include);
            if (currentInclude.ThenInclude != null)
            {
                ObjectQueryable = RecursiveInclude(ObjectQueryable, currentInclude.ThenInclude);
            }
            return ObjectQueryable;
        }

        public async Task SaveDbChangesAsync()
        {
           await _context.SaveChangesAsync();
        }
        public async Task UpdateModelsAsync(List<ModelAndState> modelsAndStates)
        {
            try
            {
                UpdateModelsWithoutSaveChanges(modelsAndStates);
                var changetracker = _context.ChangeTracker.Entries();
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateModelsWithoutSaveChanges(List<ModelAndState> modelsAndStates)
        {
            try
            {
                foreach (var ms in modelsAndStates)
                {
                    if (ms.Model != null)
                    {
                        _context.Entry(ms.Model).State = ms.StateEnum;
                    }

                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

    }
}
