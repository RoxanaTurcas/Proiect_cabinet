using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VetCare.Data;
using VetCare.Models;

namespace VetCare.Pages.Reviews
{
    public class IndexModel : PageModel
    {
        private readonly VetCare.Data.ApplicationDbContext _context;

        public IndexModel(VetCare.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Review> Review { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Reviews != null)
            {
                Review = await _context.Reviews
              .Include(r => r.Client)
              .Include(r => r.Medic).ToListAsync();
            }
        }
    }
}
