using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VetCare.Models
{
    public class Animal
    {
        [Key]
        public int PetId { get; set; } 

        public string Nume { get; set; } 

        public string Specie { get; set; } 

        public string Rasa { get; set; } 

        public int Varsta { get; set; } 
        public string Gen { get; set; }

        public string ProprietarId { get; set; }
        public virtual Users Proprietar { get; set; }

        public virtual ICollection<Programare> Programari { get; set; }
        public virtual ICollection<Vaccinare> Vaccinari { get; set; }
    }
}
