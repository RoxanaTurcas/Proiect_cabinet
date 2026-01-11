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

namespace VetCare.Pages.Animale
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
        public Animal Animal { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Animal = await _context.Animale
                .Include(a => a.Proprietar)
                .FirstOrDefaultAsync(m => m.PetId == id);

            if (Animal == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animal = await _context.Animale.FindAsync(id);

            if (animal != null)
            {
                Animal = animal;
                _context.Animale.Remove(Animal); 
                await _context.SaveChangesAsync(); 
            }

            return RedirectToPage("./Index");
        }
    }
}