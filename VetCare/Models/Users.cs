using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.ComponentModel.DataAnnotations;

namespace VetCare.Models
{
    public class Users : IdentityUser
    {
        [Key]
        public string ?Nume { get; set; } 

            public string ?Prenume { get; set; } 

            public string Rol { get; set; } 

            public virtual ICollection<Animal> AnimaleDetinute { get; set; }

            public virtual ICollection<Programare> ProgramariMedic { get; set; }

            public virtual ICollection<Review> ReviewuriDate { get; set; } 
            public virtual ICollection<Review> ReviewuriPrimite { get; set; }
        }
    }
