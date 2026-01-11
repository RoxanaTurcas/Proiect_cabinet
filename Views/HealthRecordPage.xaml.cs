using Proiect.Models;
using Proiect.Services;

namespace Proiect.Views;

public partial class HealthRecordPage : ContentPage
{
    private Pet _currentPet;
    private User _loggedUser;
    private DatabaseService _dbService;

    // Constructorul primeste Animalul si Utilizatorul curent
    public HealthRecordPage(Pet pet, User user)
    {
        InitializeComponent();

        _currentPet = pet;
        _loggedUser = user;
        _dbService = new DatabaseService();

        // Setup UI
        PetNameLabel.Text = $"Carnet: {_currentPet.Nume} 🐾";

        // Daca e medic, aratam zona de adaugare
        if (_loggedUser.Rol == "Medic")
        {
            AddVaccineSection.IsVisible = true;
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadVaccines();
    }

    private async Task LoadVaccines()
    {
        var vaccines = await _dbService.GetVaccinationsByPet(_currentPet.PetId);
        VaccineCollection.ItemsSource = vaccines;
    }

    private async void OnAddVaccineClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(VaccineNameEntry.Text))
        {
            await DisplayAlert("Eroare", "Introdu numele vaccinului!", "OK");
            return;
        }

        var newVaccine = new Vaccination
        {
            PetId = _currentPet.PetId,
            Denumire = VaccineNameEntry.Text,
            DataVaccin = VaccineDatePicker.Date
        };

        await _dbService.AddVaccination(newVaccine);

        // Curatam campurile si reincarcam lista
        VaccineNameEntry.Text = string.Empty;
        await LoadVaccines();

        await DisplayAlert("Succes", "Vaccinul a fost înregistrat!", "OK");
    }
}