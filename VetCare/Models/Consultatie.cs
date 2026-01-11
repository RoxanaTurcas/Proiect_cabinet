using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VetCare.Models
{
    public class Consultatie
    {
        [Key]
        public int ConsultatieId { get; set; } 
        public int ProgramareId { get; set; }
        public virtual Programare Programare { get; set; }

        public string Diagnostic { get; set; } // [cite: 50]
        public string Tratament { get; set; } // [cite: 51]
        public string Observatii { get; set; } // [cite: 52]

        public DateTime Data { get; set; } // [cite: 53]
    }
}
