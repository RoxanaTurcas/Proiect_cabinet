using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using VetCare.Data;
using VetCare.Models;
using System.Security.Claims;

namespace VetCare.Pages.Reviews
{
    public class CreateModel : PageModel
    {
        private readonly VetCare.Data.ApplicationDbContext _context;

        public CreateModel(VetCare.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            ViewData["VetId"] = new SelectList(_context.Users, "Id", "Email");
            return Page();
        }

        [BindProperty]
        public Review Review { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Review.ClientId = currentUserId;

            ModelState.Remove("Review.ClientId");
            ModelState.Remove("Review.Medic");
            ModelState.Remove("Review.Client");
            ModelState.Remove("Review.Vet");
            ModelState.Remove("Review.Veterinar");

            if (!ModelState.IsValid)
            {
                ViewData["VetId"] = new SelectList(_context.Users, "Id", "Email");
                return Page();
            }

            _context.Reviews.Add(Review);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}