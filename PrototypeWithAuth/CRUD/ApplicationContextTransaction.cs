using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using PrototypeWithAuth.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.CRUD
{
    public class ApplicationContextTransaction
    {
        private ApplicationDbContext _context;
        public ApplicationContextTransaction(ApplicationDbContext _context)
        {
            this._context = _context;
        }

        public IDbContextTransaction Transaction { get { return _context.Database.BeginTransaction(); } } 
    }
}
