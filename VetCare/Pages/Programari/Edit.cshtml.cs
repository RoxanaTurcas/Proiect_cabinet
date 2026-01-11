using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VetCare.Data;
using VetCare.Models;

namespace VetCare.Pages.Programari
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
        public Programare Programare { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var programare = await _context.Programari.FirstOrDefaultAsync(m => m.ProgramareId == id);

            if (programare == null)
            {
                return NotFound();
            }
            Programare = programare;

            ViewData["PetId"] = new SelectList(_context.Animale, "PetId", "Nume");
            ViewData["VetId"] = new SelectList(_context.Users, "Id", "Email");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("Programare.Animal");
            ModelState.Remove("Programare.Vet");
            ModelState.Remove("Programare.Veterinar");
            ModelState.Remove("Programare.Medic");
            ModelState.Remove("Programare.Consultatie");
            ModelState.Remove("Programare.Proprietar");

            if (!ModelState.IsValid)
            {
                ViewData["PetId"] = new SelectList(_context.Animale, "PetId", "Nume");
                ViewData["VetId"] = new SelectList(_context.Users, "Id", "Email");
                return Page();
            }

            _context.Attach(Programare).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProgramareExists(Programare.ProgramareId))
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

        private bool ProgramareExists(int id)
        {
            return _context.Programari.Any(e => e.ProgramareId == id);
        }
    }
}