using Proiect.Models;
using Proiect.Services;

namespace Proiect.Views;

public partial class AddPetPage : ContentPage
{
    private readonly DatabaseService _dbService;
    private readonly User _proprietar; // Userul care este logat acum

    // Primim DB si Proprietarul prin constructor
    public AddPetPage(DatabaseService dbService, User proprietar)
    {
        InitializeComponent();
        _dbService = dbService;
        _proprietar = proprietar;
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        // 1. Validari
        if (string.IsNullOrWhiteSpace(NumeEntry.Text) ||
            SpeciePicker.SelectedIndex == -1)
        {
            await DisplayAlert("Eroare", "Te rog introdu numele și specia.", "OK");
            return;
        }

        // Convertim varsta la numar (daca e gol punem 0)
        int.TryParse(VarstaEntry.Text, out int varsta);

        // 2. Creare obiect Pet
        var newPet = new Pet
        {
            Nume = NumeEntry.Text,
            Specie = SpeciePicker.SelectedItem.ToString(),
            Rasa = RasaEntry.Text,
            Varsta = varsta,
            Gen = GenPicker.SelectedItem?.ToString(), // ?. inseamna ca nu crapa daca e null

            // AICI facem legatura: Animalul apartine userului logat
            ProprietarId = _proprietar.UserId
        };

        // 3. Salvare in Baza de Date
        await _dbService.AddPet(newPet);

        await DisplayAlert("Succes", $"{newPet.Nume} a fost adăugat!", "OK");

        // Ne intoarcem la Dashboard
        await Navigation.PopAsync();
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}