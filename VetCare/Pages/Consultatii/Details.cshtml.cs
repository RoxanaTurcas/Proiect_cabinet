using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VetCare.Data;
using VetCare.Models;

namespace VetCare.Pages.Consultatii
{
    public class DetailsModel : PageModel
    {
        private readonly VetCare.Data.ApplicationDbContext _context;

        public DetailsModel(VetCare.Data.ApplicationDbContext context)
        {
            _context = context;
        }

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
                            .FirstOrDefaultAsync(m => m.ConsultatieId == id); if (consultatie == null)
            {
                return NotFound();
            }
            else
            {
                Consultatie = consultatie;
            }
            return Page();
        }
    }
}
