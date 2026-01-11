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
using System.Security.Claims; 

namespace VetCare.Pages.Programari
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
            ViewData["PetId"] = new SelectList(_context.Animale, "PetId", "Nume");

            ViewData["VetId"] = new SelectList(_context.Users, "Id", "Email");

            return Page();
        }

        [BindProperty]
        public Programare Programare { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("Programare.Animal");
            ModelState.Remove("Programare.Vet");
            ModelState.Remove("Programare.Veterinar");
            ModelState.Remove("Programare.Proprietar");
            ModelState.Remove("Programare.Medic");
            ModelState.Remove("Programare.Consultatie");

            if (!ModelState.IsValid)
            {
                ViewData["PetId"] = new SelectList(_context.Animale, "PetId", "Nume");
                ViewData["VetId"] = new SelectList(_context.Users, "Id", "Email");

                return Page();
            }

            _context.Programari.Add(Programare);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}