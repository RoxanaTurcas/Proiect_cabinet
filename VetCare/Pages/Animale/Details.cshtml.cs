using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VetCare.Data;
using VetCare.Models;

namespace VetCare.Pages.Animale
{
    public class DetailsModel : PageModel
    {
        private readonly VetCare.Data.ApplicationDbContext _context;

        public DetailsModel(VetCare.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Animal Animal { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Animal = await _context.Animale
                .Include(m => m.Proprietar)  
                .FirstOrDefaultAsync(m => m.PetId == id);

            if (Animal == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
