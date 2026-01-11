using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VetCare.Models
{
    public class Programare
    {
        [Key]
        public int ProgramareId { get; set; } 
        public DateTime Data { get; set; } 
        public string Status { get; set; }
        public string Scop { get; set; }
        public int PetId { get; set; }
        public virtual Animal Animal { get; set; }
        public string VetId { get; set; }
        public virtual Users Medic { get; set; }
        public virtual Consultatie Consultatie { get; set; }
    }
}
