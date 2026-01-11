using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VetCare.Data;
using VetCare.Models;

namespace VetCare.Pages.Vaccinuri
{
    public class DetailsModel : PageModel
    {
        private readonly VetCare.Data.ApplicationDbContext _context;

        public DetailsModel(VetCare.Data.ApplicationDbContext context)
        {
            _context = context;
        }

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
    }
}
