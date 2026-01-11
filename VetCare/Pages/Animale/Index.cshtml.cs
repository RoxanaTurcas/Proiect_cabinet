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
    public class IndexModel : PageModel
    {
        private readonly VetCare.Data.ApplicationDbContext _context;

        public IndexModel(VetCare.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Animal> Animal { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Animal = await _context.Animale
                .Include(a => a.Proprietar).ToListAsync();
        }
    }
}
