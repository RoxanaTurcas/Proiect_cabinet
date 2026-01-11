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

namespace VetCare.Pages.Consultatii
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
        public Consultatie Consultatie { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var consultatie = await _context.Consultatii
                .Include(c => c.Programare)
                .ThenInclude(p => p.Animal)
                .FirstOrDefaultAsync(m => m.ConsultatieId == id);

            if (consultatie == null)
            {
                return NotFound();
            }
            else
            {
                Consultatie = consultatie;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var consultatie = await _context.Consultatii.FindAsync(id);
            if (consultatie != null)
            {
                Consultatie = consultatie;
                _context.Consultatii.Remove(Consultatie);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}