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

namespace VetCare.Pages.Consultatii
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
            var listaProgramari = _context.Programari
                .Include(p => p.Animal)
                .Select(p => new
                {
                    ID = p.ProgramareId,
                    TextAfisat = p.Animal.Nume + " - " + p.Data
                })
                .ToList();

            ViewData["ProgramareId"] = new SelectList(listaProgramari, "ID", "TextAfisat");
            return Page();
        }

        [BindProperty]
        public Consultatie Consultatie { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("Consultatie.Programare");

            if (!ModelState.IsValid)
            {
                var listaProgramari = _context.Programari
                    .Include(p => p.Animal)
                    .Select(p => new
                    {
                        ID = p.ProgramareId,
                        TextAfisat = p.Animal.Nume + " - " + p.Data
                    })
                    .ToList();

                ViewData["ProgramareId"] = new SelectList(listaProgramari, "ID", "TextAfisat");
                return Page();
            }

            _context.Consultatii.Add(Consultatie);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}