using System;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proiect.Models
{
    public class Review
    {
        [PrimaryKey, AutoIncrement]
        public int ReviewId { get; set; }
        public int ProgramareId { get; set; }
        public int ClientId { get; set; }
        public int VetId { get; set; }
        public int Nota { get; set; }
        public string Comentariu { get; set; }
        public DateTime DataReview { get; set; }
        [Ignore]
        public string NumeClientDisplay { get; set; }
    }
}
