using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite;   
using System.Threading.Tasks;

namespace Proiect.Models
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int UserId { get; set; }
        public string Nume { get; set; }
        public string Prenume { get; set; }
        [Unique]
        public string Email { get; set; }
        public string Parola { get; set; }
        public string Telefon { get; set; }
        public string Rol { get; set; } // Medic / Client
    }
}
