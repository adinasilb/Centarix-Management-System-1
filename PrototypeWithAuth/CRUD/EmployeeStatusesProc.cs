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

        public void CreateWithoutSaving(EmployeeStatus employeeStatus)
        {
            _context.Add(employeeStatus);
        }
    }
}
