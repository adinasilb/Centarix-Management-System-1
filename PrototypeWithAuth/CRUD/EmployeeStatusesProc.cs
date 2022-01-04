using Microsoft.EntityFrameworkCore;
using PrototypeWithAuth.Data;
using PrototypeWithAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.CRUD
{
    public class EmployeeStatusesProc : ApplicationDbContextProc<EmployeeStatus>
    {
        public EmployeeStatusesProc(ApplicationDbContext context, bool FromBase = false ) : base(context)
        {
            if (!FromBase) { base.InstantiateProcs(); }
        }

        public void UpdateWithoutSaving(EmployeeStatus employeeStatus)
        {
            _context.Entry(employeeStatus).State = EntityState.Modified;
            //_context.Add(employeeStatus);
        }
    }
}
