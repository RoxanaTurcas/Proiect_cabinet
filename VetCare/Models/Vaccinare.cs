using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace VetCare.Models
{
    public class Vaccinare
    {
        [Key]
        public int VaccinId { get; set; } 

        public string Denumire { get; set; }

        public DateTime DataVaccin { get; set; }

        public int PetId { get; set; } 

        [ForeignKey("PetId")] 
        public virtual Animal? Animal { get; set; }
    }
}
