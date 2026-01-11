using Proiect.Models;
using Proiect.Services;
using Plugin.LocalNotification;
using static System.Collections.Specialized.NameObjectCollectionBase;

namespace Proiect.Views;

public partial class DashboardPage : ContentPage
{
    private readonly DatabaseService _dbService;
    private readonly User _loggedUser;

    // Constructorul primeste userul logat
    public DashboardPage(DatabaseService dbService, User user)
    {
        InitializeComponent();
        _dbService = dbService;
        _loggedUser = user;

        // Setam mesajul de bun venit
        WelcomeLabel.Text = $"Salut, {_loggedUser.Prenume}!";

        // Activam sectiunea corecta
        if (_loggedUser.Rol == "Medic")
        {
            MedicSection.IsVisible = true;
            ClientSection.IsVisible = false;
        }
        else
        {
            ClientSection.IsVisible = true;
            MedicSection.IsVisible = false;
        }
    }

    // Aceasta metoda se apeleaza de fiecare data cand pagina apare pe ecran
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadData();

        await ScheduleNotifications();
    }
    private async Task ScheduleNotifications()
    {
        // Notificam doar clientii
        if (_loggedUser.Rol != "Client")
            return;

        // Cerem permisiune (necesar pt Android 13+)
        if (await LocalNotificationCenter.Current.AreNotificationsEnabled() == false)
        {
            await LocalNotificationCenter.Current.RequestNotificationPermission();
        }

        // Luam programarile clientului
        var myAppointments = await _dbService.GetAppointmentsByOwner(_loggedUser.UserId);

        foreach (var app in myAppointments)
        {
            // Calculam: O zi inainte de data programata
            var dataProgramare = app.Data;
            var dataNotificare = dataProgramare.AddDays(-1);

            // Pentru TEST: Daca vrei sa testezi ACUM, scoate comentariul de la linia de mai jos:
             dataNotificare = DateTime.Now.AddSeconds(10); 

            // Conditii: Programare CONFIRMATA + Data notificarii e in viitor
            if (app.Status == "Confirmata" && dataNotificare > DateTime.Now)
            {
                var request = new NotificationRequest
                {
                    NotificationId = app.ProgramareId, // ID Unic
                    Title = "Reamintire Veterinar 🐾",
                    Description = $"Mâine ai programare pentru {app.NumeAnimalDisplay} la ora {dataProgramare:HH:mm}!",
                    BadgeNumber = 1,
                    Schedule = new NotificationRequestSchedule
                    {
                        NotifyTime = dataNotificare
                    }
                };

                await LocalNotificationCenter.Current.Show(request);
            }
        }
    }
    private async Task LoadData()
    {
        // ---------------------------------------------------------
        // LOGICA PENTRU CLIENT
        // ---------------------------------------------------------
        if (_loggedUser.Rol == "Client")
        {
            // 1. Incarcam animalele
            var animals = await _dbService.GetPetsByOwner(_loggedUser.UserId);
            PetsCollection.ItemsSource = animals;

            // 2. Incarcam programarile clientului (ca sa vada statusul si butonul de recenzie)
            var myAppointments = await _dbService.GetAppointmentsByOwner(_loggedUser.UserId);
            ClientAppointmentsCollection.ItemsSource = myAppointments;
        }

        // ---------------------------------------------------------
        // LOGICA PENTRU MEDIC
        // ---------------------------------------------------------
        else if (_loggedUser.Rol == "Medic")
        {
            // 1. Incarcam programarile pe care trebuie sa le onoreze medicul
            var appointments = await _dbService.GetAppointmentsByVet(_loggedUser.UserId);
            AppointmentsCollection.ItemsSource = appointments;

            // 2. Calculam si afisam media notelor in Titlu
            double media = await _dbService.GetVetAverageRating(_loggedUser.UserId);
            Title = $"Dashboard Medic (Rating: {media:F1} ⭐)";

            // 3. Incarcam lista de recenzii primite
            var reviews = await _dbService.GetReviewsForVet(_loggedUser.UserId);
            ReviewsCollection.ItemsSource = reviews;
        }
    }

    private async void OnAddPetClicked(object sender, EventArgs e)
    {
        // Navigam la pagina de adaugare si trimitem userul curent (Proprietarul)
        await Navigation.PushAsync(new AddPetPage(_dbService, _loggedUser));
    }

    private async void OnRefreshClicked(object sender, EventArgs e)
    {
        await LoadData();
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        // Ne intoarcem la pagina de Login
        await Navigation.PopToRootAsync();
    }
    private async void OnRequestAppointmentClicked(object sender, EventArgs e)
    {
        // Navigam spre pagina de programare
        await Navigation.PushAsync(new RequestAppointmentPage(_dbService, _loggedUser));
    }
    private async void OnConfirmClicked(object sender, EventArgs e)
    {
        // 1. Aflam pe ce buton s-a apasat
        var button = (Button)sender;

        // 2. Extragem programarea asociata acelui buton (prin CommandParameter)
        var appointment = (Appointment)button.CommandParameter;

        // 3. Modificam statusul
        appointment.Status = "Confirmata";

        // 4. Salvam in baza de date
        await _dbService.UpdateAppointment(appointment);

        // 5. Reincarcam lista ca sa vedem modificarea
        await LoadData();

        await DisplayAlert("Succes", "Programarea a fost confirmată!", "OK");
    }

    private async void OnRejectClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var appointment = (Appointment)button.CommandParameter;

        // Putem cere o confirmare inainte sa stergem/anulam
        bool answer = await DisplayAlert("Atenție", "Sigur vrei să anulezi această programare?", "Da", "Nu");

        if (answer)
        {
            appointment.Status = "Anulata";
            await _dbService.UpdateAppointment(appointment);
            await LoadData();
        }
    }
    private async void OnOpenHealthRecordClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var selectedPet = (Pet)button.CommandParameter;

        // Navigam catre pagina de vaccinuri, trimitand Animalul si Userul logat
        await Navigation.PushAsync(new HealthRecordPage(selectedPet, _loggedUser));
    }
    private async void OnDoctorOpenHealthRecordClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var appointment = (Appointment)button.CommandParameter;

        // 1. Gasim animalul asociat programarii
        var pet = await _dbService.GetPetById(appointment.PetId);

        if (pet != null)
        {
            // 2. Deschidem carnetul (fiind logati ca Medic, vom vedea formularul de adaugare)
            await Navigation.PushAsync(new HealthRecordPage(pet, _loggedUser));
        }
        else
        {
            await DisplayAlert("Eroare", "Nu am găsit animalul!", "OK");
        }
    }
    private async void OnLeaveReviewClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var app = (Appointment)button.CommandParameter;

        // 1. Verificam daca e confirmata
        if (app.Status != "Confirmata")
        {
            await DisplayAlert("Info", "Poți lăsa recenzii doar după ce programarea a fost confirmată și realizată.", "OK");
            return;
        }

        // 2. Verificam daca a lasat deja review
        bool exists = await _dbService.ReviewExists(app.ProgramareId);
        if (exists)
        {
            await DisplayAlert("Info", "Ai lăsat deja o recenzie pentru această vizită!", "OK");
            return;
        }

        // 3. Cerem Nota (Input simplu)
        string notaString = await DisplayActionSheet("Ce notă acorzi medicului?", "Anulează", null, "5 ⭐", "4 ⭐", "3 ⭐", "2 ⭐", "1 ⭐");
        if (notaString == "Anulează" || notaString == null) return;

        int nota = int.Parse(notaString.Substring(0, 1)); // Luam prima cifra (5, 4 etc)

        // 4. Cerem Comentariu
        string comentariu = await DisplayPromptAsync("Feedback", "Scrie un scurt comentariu:");

        // 5. Salvam
        var review = new Review
        {
            ProgramareId = app.ProgramareId,
            VetId = app.VetId,
            ClientId = _loggedUser.UserId,
            Nota = nota,
            Comentariu = comentariu ?? "", // Daca e null, punem gol
            DataReview = DateTime.Now
        };

        await _dbService.AddReview(review);
        await DisplayAlert("Mulțumim!", "Recenzia ta a fost înregistrată.", "OK");
    }
    // --- EDITARE ANIMAL ---
    private async void OnEditPetClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var pet = (Pet)button.CommandParameter;

        // Navigam catre pagina de editare completa, trimitand animalul selectat
        await Navigation.PushAsync(new EditPetPage(pet));
    }

    // --- STERGERE ANIMAL ---
    private async void OnDeletePetClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var pet = (Pet)button.CommandParameter;

        // Cerem confirmare ca sa nu stearga din greseala
        bool confirm = await DisplayAlert("Ștergere",
                                          $"Ești sigur că vrei să ștergi animalul {pet.Nume}?",
                                          "Da, Șterge",
                                          "Nu");

        if (confirm)
        {
            // Stergem din baza de date
            await _dbService.DeletePet(pet);

            // Reincarcam lista
            await LoadData();
        }
    }
   
}