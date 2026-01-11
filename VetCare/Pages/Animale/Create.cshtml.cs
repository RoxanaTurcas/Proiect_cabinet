using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using VetCare.Data;
using VetCare.Models;

namespace VetCare.Pages.Animale
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly VetCare.Data.ApplicationDbContext _context;

        public CreateModel(VetCare.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["ProprietarId"] = new SelectList(_context.Users, "Id", "Id");
            return Page();
        }

        [BindProperty]
        public Animal Animal { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Animal.ProprietarId = userId;

            ModelState.Remove("Animal.ProprietarId");
            ModelState.Remove("Animal.Proprietar");
            ModelState.Remove("Proprietar");
            ModelState.Remove("Animal.Programari");
            ModelState.Remove("Animal.Vaccinari");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Animale.Add(Animal);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
