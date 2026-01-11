using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using VetCare.Data;
using VetCare.Models;

namespace VetCare.Pages.Reviews
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly VetCare.Data.ApplicationDbContext _context;

        public EditModel(VetCare.Data.ApplicationDbContext context)
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

            var review = await _context.Reviews.FirstOrDefaultAsync(m => m.ReviewId == id);
            if (review == null)
            {
                return NotFound();
            }
            Review = review;

            ViewData["ClientId"] = new SelectList(_context.Users, "Id", "Email");
            ViewData["VetId"] = new SelectList(_context.Users, "Id", "Email");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("Review.Client");
            ModelState.Remove("Review.Vet");
            ModelState.Remove("Review.Veterinar");
            ModelState.Remove("Review.Medic");

            if (!ModelState.IsValid)
            {
                ViewData["ClientId"] = new SelectList(_context.Users, "Id", "Email");
                ViewData["VetId"] = new SelectList(_context.Users, "Id", "Email");
                return Page();
            }

            _context.Attach(Review).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewExists(Review.ReviewId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ReviewExists(int id)
        {
            return _context.Reviews.Any(e => e.ReviewId == id);
        }
    }
}