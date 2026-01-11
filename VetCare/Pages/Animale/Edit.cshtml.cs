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

namespace VetCare.Pages.Animale
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
        public Animal Animal { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animal = await _context.Animale.FirstOrDefaultAsync(m => m.PetId == id);

            if (animal == null)
            {
                return NotFound();
            }
            Animal = animal;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            _context.Attach(Animal).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnimalExists(Animal.PetId)) 
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

        private bool AnimalExists(int id)
        {
            return _context.Animale.Any(e => e.PetId == id); 
        }
    }
}