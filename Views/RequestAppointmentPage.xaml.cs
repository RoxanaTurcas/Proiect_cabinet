using Proiect.Models;
using Proiect.Services;

namespace Proiect.Views;

public partial class RequestAppointmentPage : ContentPage
{
    private readonly DatabaseService _dbService;
    private readonly User _loggedUser; // Clientul

    public RequestAppointmentPage(DatabaseService dbService, User user)
    {
        InitializeComponent();
        _dbService = dbService;
        _loggedUser = user;

        AppDatePicker.MinimumDate = DateTime.Today;

    }

    // Aceasta metoda se apeleaza automat inainte sa apara pagina
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadPickersData();
    }

    private async Task LoadPickersData()
    {
        // 1. Incarcam animalele clientului curent
        var myPets = await _dbService.GetPetsByOwner(_loggedUser.UserId);
        PetPicker.ItemsSource = myPets;

        // 2. Incarcam lista de medici
        // (Asigura-te ca ai metoda GetVets() in DatabaseService, ti-am dat-o anterior)
        var vets = await _dbService.GetVets();
        VetPicker.ItemsSource = vets;
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        // 1. Validari
        if (PetPicker.SelectedItem == null || VetPicker.SelectedItem == null || ScopPicker.SelectedIndex == -1)
        {
            await DisplayAlert("Eroare", "Te rog completează toate câmpurile.", "OK");
            return;
        }

        // Extragem obiectele selectate
        var selectedPet = (Pet)PetPicker.SelectedItem;
        var selectedVet = (User)VetPicker.SelectedItem;

        // Combinam Data cu Ora
        DateTime fullDate = AppDatePicker.Date + AppTimePicker.Time;

        // Validare data (sa nu fie in trecut)
        if (fullDate < DateTime.Now)
        {
            await DisplayAlert("Eroare", "Nu poți face o programare în trecut!", "OK");
            return;
        }

        // 2. Creare Programare
        var newAppointment = new Appointment
        {
            PetId = selectedPet.PetId,
            VetId = selectedVet.UserId, // ID-ul medicului ales
            Data = fullDate,
            Scop = ScopPicker.SelectedItem.ToString(),
            Status = "Solicitata" // Status initial
        };

        // 3. Salvare
        await _dbService.AddAppointment(newAppointment);

        await DisplayAlert("Succes", "Solicitarea a fost trimisă!", "OK");
        await Navigation.PopAsync();
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}