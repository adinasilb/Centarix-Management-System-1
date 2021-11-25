using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PrototypeWithAuth.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrototypeWithAuth.CRUD
{
    public class CountriesProc : ApplicationDbContextProc
    {
        public CountriesProc(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : base(context, userManager)
        {
        }

        public IQueryable<Models.Country> Read()
        {
            return _context.Countries.AsNoTracking().AsQueryable();
        }

        public List<SelectListItem> ReadAsSelectList()
        {
            var returnList = new List<SelectListItem>();
            foreach (var country in _context.Countries)
            {
                returnList.Add(new SelectListItem() { Text = country.CountryName, Value = country.CountryID.ToString() });
            }
            return returnList;
        }
    }
}
