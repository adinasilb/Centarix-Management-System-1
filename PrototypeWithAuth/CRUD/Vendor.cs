using Microsoft.EntityFrameworkCore;
using PrototypeWithAuth.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.CRUD
{
    public class Vendor
    {
        private readonly ApplicationDbContext _context;
        public Vendor(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<Models.Vendor> Read()
        {
            return _context.Vendors.AsNoTracking().AsQueryable();
        }

        public IQueryable<Models.Vendor> Read(int CategoryTypeID)
        {
            return _context.Vendors
                .Where(v => v.VendorCategoryTypes.Where(vc => vc.CategoryTypeID == CategoryTypeID).Count() > 0)
                .AsQueryable();
        }
    }
}
