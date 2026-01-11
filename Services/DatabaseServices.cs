using SQLite;
using Proiect.Models;
// Aceste using-uri sunt necesare pentru fisiere si liste
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Proiect.Services
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection _database;

        // 1. Initializare: Crearea Bazei de Date si a Tabelor
        async Task Init()
        {
            if (_database != null)
                return; // Daca suntem deja conectati, nu mai facem nimic

            // Calea unde se salveaza fisierul pe telefon
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "CabinetVet.db");

            _database = new SQLiteAsyncConnection(dbPath);

            // Creem tabelele (Daca exista deja, SQLite le ignora, deci e safe)
            await _database.CreateTableAsync<User>();
            await _database.CreateTableAsync<Pet>();
            await _database.CreateTableAsync<Appointment>();
            await _database.CreateTableAsync<Review>();
            await _database.CreateTableAsync<Vaccination>();
        }

        // =======================================================
        // ZONA DE UTILIZATORI (Login & Register)
        // =======================================================

        public async Task<int> RegisterUser(User user)
        {
            await Init();
            return await _database.InsertAsync(user);
        }

        public async Task<User> Login(string email, string parola)
        {
            await Init();
            // Cautam primul user care are emailul SI parola potrivite
            return await _database.Table<User>()
                                  .Where(u => u.Email == email && u.Parola == parola)
                                  .FirstOrDefaultAsync();
        }

        public async Task<List<User>> GetVets() // Doar pentru a lua lista de medici
        {
            await Init();
            return await _database.Table<User>().Where(u => u.Rol == "Medic").ToListAsync();
        }

        // =======================================================
        // ZONA DE ANIMALE (PETS)
        // =======================================================

        public async Task<int> AddPet(Pet pet)
        {
            await Init();
            return await _database.InsertAsync(pet);
        }

        public async Task<List<Pet>> GetPetsByOwner(int ownerId)
        {
            await Init();
            // Returnam doar animalele utilizatorului logat
            return await _database.Table<Pet>()
                                  .Where(p => p.ProprietarId == ownerId)
                                  .ToListAsync();
        }

        // =======================================================
        // ZONA DE PROGRAMARI (APPOINTMENTS)
        // =======================================================

        public async Task<int> AddAppointment(Appointment appointment)
        {
            await Init();
            return await _database.InsertAsync(appointment);
        }

        public async Task<List<Appointment>> GetAppointmentsByVet(int vetId)
        {
            await Init();

            // 1. Luam programarile din baza
            var appointments = await _database.Table<Appointment>()
                                              .Where(a => a.VetId == vetId)
                                              .OrderBy(a => a.Data)
                                              .ToListAsync();

            // 2. Pentru fiecare programare, completam numele lipsa
            foreach (var app in appointments)
            {
                // Cautam animalul
                var pet = await _database.Table<Pet>()
                                         .Where(p => p.PetId == app.PetId)
                                         .FirstOrDefaultAsync();

                // Cautam stapanul (Clientul)
                var client = await _database.Table<User>()
                                            .Where(u => u.UserId == app.ClientId)
                                            .FirstOrDefaultAsync();

                // Le punem in variabilele de afisare
                if (pet != null)
                {
                    app.NumeAnimalDisplay = $"{pet.Nume} ({pet.Specie})";
                }
                else
                {
                    app.NumeAnimalDisplay = "Animal necunoscut";
                }

                if (client != null)
                {
                    app.NumeClientDisplay = client.Nume; 
                }
                else
                {
                    app.NumeClientDisplay = "Client necunoscut";
                }
            }

            return appointments;
        }

        public async Task<List<Appointment>> GetAppointmentsForPet(int petId)
        {
            await Init();
            return await _database.Table<Appointment>()
                                  .Where(a => a.PetId == petId)
                                  .ToListAsync();
        }

        // =======================================================
        // ZONA DE VACCINARI & REVIEWS
        // =======================================================

        public async Task<int> AddVaccination(Vaccination vacc)
        {
            await Init();
            return await _database.InsertAsync(vacc);
        }

        public async Task<List<Vaccination>> GetVaccinationsByPet(int petId)
        {
            await Init();
            return await _database.Table<Vaccination>()
                                  .Where(v => v.PetId == petId)
                                  .OrderByDescending(v => v.DataVaccin) // Cele mai recente sus
                                  .ToListAsync();
        }

        public async Task<int> AddReview(Review review)
        {
            await Init();
            return await _database.InsertAsync(review);
        }

        public async Task<List<Review>> GetReviewsForVet(int vetId)
        {
            await Init();
            return await _database.Table<Review>().Where(r => r.VetId == vetId).ToListAsync();
        }
        public async Task<bool> ReviewExists(int programareId)
        {
            await Init();
            var existing = await _database.Table<Review>()
                                          .Where(r => r.ProgramareId == programareId)
                                          .FirstOrDefaultAsync();
            return existing != null;
        }

        // 3. Calculeaza media notelor unui medic
        public async Task<double> GetVetAverageRating(int vetId)
        {
            await Init();
            var reviews = await _database.Table<Review>().Where(r => r.VetId == vetId).ToListAsync();

            if (reviews.Count == 0) return 0; // Daca nu are recenzii

            return reviews.Average(r => r.Nota);
        }
        // Metoda pentru a actualiza o programare (ex: schimbare status)
        public async Task UpdateAppointment(Appointment appointment)
        {
            await Init();
            await _database.UpdateAsync(appointment);
        }
        // Returneaza toate programarile animalelor detinute de un anumit proprietar
        public async Task<List<Appointment>> GetAppointmentsByOwner(int ownerId)
        {
            await Init();

            // 1. Luam toate animalele proprietarului
            var pets = await _database.Table<Pet>().Where(p => p.ProprietarId == ownerId).ToListAsync();

            var allAppointments = new List<Appointment>();

            // 2. Pentru fiecare animal, luam programarile lui
            foreach (var pet in pets)
            {
                var apps = await _database.Table<Appointment>().Where(a => a.PetId == pet.PetId).ToListAsync();

                // (Optional) Completam numele animalului ca sa il putem afisa frumos in lista
                foreach (var app in apps)
                {
                    app.NumeAnimalDisplay = pet.Nume;
                }

                allAppointments.AddRange(apps);
            }

            // Le returnam sortate dupa data (cele mai recente primele)
            return allAppointments.OrderBy(a => a.Data).ToList();
        }
        // Gaseste un animal dupa ID (ne trebuie cand medicul da click din programare)
        public async Task<Pet> GetPetById(int petId)
        {
            await Init();
            return await _database.Table<Pet>().Where(p => p.PetId == petId).FirstOrDefaultAsync();
        }
        // --- METODE PENTRU EDITARE SI STERGERE ANIMAL ---

        // 1. Actualizeaza datele unui animal existent
        public async Task UpdatePet(Pet pet)
        {
            await Init();
            await _database.UpdateAsync(pet);
        }

        // 2. Sterge un animal definitiv
        public async Task DeletePet(Pet pet)
        {
            await Init();
            await _database.DeleteAsync(pet);
        }
    }
}

           