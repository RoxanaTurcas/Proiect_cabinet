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
    public class DetailsModel : PageModel
    {
        private readonly VetCare.Data.ApplicationDbContext _context;

        public DetailsModel(VetCare.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Review Review { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews
                            .Include(r => r.Client)
                            .Include(r => r.Medic)
                            .FirstOrDefaultAsync(m => m.ReviewId == id); if (review == null)
            {
                return NotFound();
            }
            if (review == null)
            {
                return NotFound();
            }
            else
            {
                Review = review;
                return Page();
            }
        }
    }
}
