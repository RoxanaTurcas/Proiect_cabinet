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

namespace VetCare.Pages.Vaccinuri
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
        public Vaccinare Vaccinare { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vaccinare =  await _context.Vaccinuri.FirstOrDefaultAsync(m => m.VaccinId == id);
            if (vaccinare == null)
            {
                return NotFound();
            }
            Vaccinare = vaccinare;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Vaccinare).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VaccinareExists(Vaccinare.VaccinId))
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

        private bool VaccinareExists(int id)
        {
            return _context.Vaccinuri.Any(e => e.VaccinId == id);
        }
    }
}
