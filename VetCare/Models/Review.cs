using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VetCare.Models
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; } 
        public int Rating { get; set; }
        public string Comentariu { get; set; } 
        public DateTime Data { get; set; } 
        public string ClientId { get; set; }

        [ForeignKey("ClientId")]
        [InverseProperty("ReviewuriDate")] 
        public virtual Users Client { get; set; }
        public string VetId { get; set; }

        [ForeignKey("VetId")]
        [InverseProperty("ReviewuriPrimite")] 
        public virtual Users Medic { get; set; }
    }
}
