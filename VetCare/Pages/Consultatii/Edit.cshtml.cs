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
    public class EditModel : PageModel
    {
        private readonly VetCare.Data.ApplicationDbContext _context;

        public EditModel(VetCare.Data.ApplicationDbContext context)
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

            var consultatie =  await _context.Consultatii.FirstOrDefaultAsync(m => m.ConsultatieId == id);
            if (consultatie == null)
            {
                return NotFound();
            }
            Consultatie = consultatie;
           ViewData["ProgramareId"] = new SelectList(_context.Programari, "ProgramareId", "ProgramareId");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Consultatie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConsultatieExists(Consultatie.ConsultatieId))
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

        private bool ConsultatieExists(int id)
        {
            return _context.Consultatii.Any(e => e.ConsultatieId == id);
        }
    }
}
