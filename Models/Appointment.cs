using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite;
using System.Threading.Tasks;

namespace Proiect.Models
{
    public class Appointment
    {
        [PrimaryKey, AutoIncrement]
        public int ProgramareId { get; set; }
        public int PetId { get; set; }
        public int VetId { get; set; }
        public int ClientId { get; set; }
        public DateTime Data { get; set; }
        public string Status { get; set; }
        public string Scop { get; set; }

        [Ignore]
        public string NumeAnimalDisplay { get; set; }
        [Ignore]
        public string NumeClientDisplay { get; set; } 
    }
}
