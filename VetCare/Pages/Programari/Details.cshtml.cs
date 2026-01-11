using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VetCare.Data;
using VetCare.Models;

namespace VetCare.Pages.Programari
{
    public class DetailsModel : PageModel
    {
        private readonly VetCare.Data.ApplicationDbContext _context;

        public DetailsModel(VetCare.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Programare Programare { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var programare = await _context.Programari
                .Include(p => p.Animal)
                .Include(p => p.Medic)
                .FirstOrDefaultAsync(m => m.ProgramareId == id);

            if (programare == null)
            {
                return NotFound();
            }
            else
            {
                Programare = programare;
            }
            return Page();
        }
    }
}