using Microsoft.EntityFrameworkCore.ChangeTracking;
using PrototypeWithAuth.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.CRUD
{
   
    public class ApplicationDbContextEntries
    {
        private ApplicationDbContext _context;
        public ApplicationDbContextEntries(ApplicationDbContext _context)
        {
            this._context = _context;
        }

        public IEnumerable<EntityEntry> Entries { get { return _context.ChangeTracker.Entries(); }}
    }
}
