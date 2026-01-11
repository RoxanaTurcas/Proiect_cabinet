using System;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proiect.Models
{
    public class Pet
    {
        [PrimaryKey, AutoIncrement]
        public int PetId { get; set; }
        public string Nume { get; set; }
        public string Specie { get; set; }
        public string Rasa { get; set; }
        public int Varsta { get; set; }
        public string Gen { get; set; }
        public int ProprietarId { get; set; }
    }
}
