using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using VetCare.Data;
using VetCare.Models;

namespace VetCare.Pages.Reviews
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly VetCare.Data.ApplicationDbContext _context;

        public DeleteModel(VetCare.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Review Review { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews
                .Include(r => r.Medic)    
                .Include(r => r.Client) 
                .FirstOrDefaultAsync(m => m.ReviewId == id);

            if (review == null)
            {
                return NotFound();
            }
            else
            {
                Review = review;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews.FindAsync(id);
            if (review != null)
            {
                Review = review;
                _context.Reviews.Remove(Review);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}