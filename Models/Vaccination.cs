using System;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proiect.Models
{
    public class Vaccination
    {
        [PrimaryKey, AutoIncrement]
        public int VaccinId { get; set; }
        public int PetId{ get; set; }
        public string Denumire { get; set; }
        public DateTime DataVaccin { get; set; }
    }
}
