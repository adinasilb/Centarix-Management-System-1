using PrototypeWithAuth.AppData;
using PrototypeWithAuth.AppData.UtilityModels;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.CRUD
{
    public class CentarixIDsProc : ApplicationDbContextProc<CentarixID>
    {
        public CentarixIDsProc(ApplicationDbContext context, bool FromBase = false) : base(context)
        {
            if (!FromBase) { base.InstantiateProcs(); }
        }

        public void CreateWithoutSaving(CentarixID centarixID)
        {
            _context.Add(centarixID);
        }


        public async Task<StringWithBool> AddNewCentarixID(string UserID, int StatusID)
        {
            StringWithBool ReturnVal = new StringWithBool();
            try
            {
                var oldCentarixID = this.Read(new  List<System.Linq.Expressions.Expression<Func<CentarixID, bool>>>
                { ci => ci.EmployeeID == UserID, ci => ci.IsCurrent}).FirstOrDefault();
                oldCentarixID.IsCurrent = false;
                _context.Update(oldCentarixID);
                await _context.SaveChangesAsync();

                var lastStatus = _employeeStatusesProc.Read(new List<System.Linq.Expressions.Expression<Func<EmployeeStatus, bool>>>
                { es => es.EmployeeStatusID == StatusID }).FirstOrDefault();
                var newNum = lastStatus.LastCentarixID + 1;
                var abbrev = lastStatus.Abbreviation;
                if (abbrev[1] == ' ')
                {
                    abbrev = abbrev.Substring(0, 1);
                }
                var newID = abbrev + newNum.ToString();

                var newCentarixID = new CentarixID()
                {
                    EmployeeID = UserID,
                    CentarixIDNumber = newID,
                    IsCurrent = true,
                    TimeStamp = DateTime.Now,
                    Employee = _employeesProc.Read(new List<System.Linq.Expressions.Expression<Func<Employee, bool>>>
                    { e => e.Id == UserID }).FirstOrDefault()
                };
                _context.Add(newCentarixID);
                await _context.SaveChangesAsync();

                lastStatus.LastCentarixID = newNum;
                lastStatus.LastCentarixIDTimeStamp = DateTime.Now;
                _context.Update(lastStatus);
                await _context.SaveChangesAsync();
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
