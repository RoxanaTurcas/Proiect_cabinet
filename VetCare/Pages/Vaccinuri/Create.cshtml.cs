using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using VetCare.Data;
using VetCare.Models;

namespace VetCare.Pages.Vaccinuri
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
            // AICI E CHEIA: Incarcam lista de animale (Nume) in ViewBag
            ViewData["PetId"] = new SelectList(_context.Animale, "PetId", "Nume");
            return Page();
        }

        [BindProperty]
        public Vaccinare Vaccinare { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            // Ignoram validarea pentru obiectul Animal (se face automat legatura prin PetId)
            ModelState.Remove("Vaccinare.Animal");

            if (!ModelState.IsValid)
            {
                // Daca e eroare, reincarcam lista
                ViewData["PetId"] = new SelectList(_context.Animale, "PetId", "Nume");
                return Page();
            }

            _context.Vaccinuri.Add(Vaccinare);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}