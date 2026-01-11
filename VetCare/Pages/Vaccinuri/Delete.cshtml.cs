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

namespace VetCare.Pages.Vaccinuri
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
        public Vaccinare Vaccinare { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vaccinare = await _context.Vaccinuri.FirstOrDefaultAsync(m => m.VaccinId == id);

            if (vaccinare == null)
            {
                return NotFound();
            }
            else
            {
                Vaccinare = vaccinare;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vaccinare = await _context.Vaccinuri.FindAsync(id);
            if (vaccinare != null)
            {
                Vaccinare = vaccinare;
                _context.Vaccinuri.Remove(Vaccinare);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
