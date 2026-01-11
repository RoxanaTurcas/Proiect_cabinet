using Proiect.Models;
using Proiect.Services;

namespace Proiect.Views;

public partial class EditPetPage : ContentPage
{
    private Pet _petToEdit;
    private DatabaseService _dbService;

    public EditPetPage(Pet pet)
    {
        InitializeComponent();

        _petToEdit = pet;
        _dbService = new DatabaseService();

        // 1. UMPLEM AUTOMAT CASUTELE CU DATELE EXISTENTE
        TxtNume.Text = pet.Nume;
        TxtSpecie.Text = pet.Specie;
        TxtRasa.Text = pet.Rasa;
        TxtGen.Text = pet.Gen;
        TxtVarsta.Text = pet.Varsta.ToString();
    }

    // Cand apesi pe Salveaza
    private async void OnSaveClicked(object sender, EventArgs e)
    {
        // Validare: Nu lasam sa salveze fara Nume sau Specie
        if (string.IsNullOrWhiteSpace(TxtNume.Text) || string.IsNullOrWhiteSpace(TxtSpecie.Text))
        {
            await DisplayAlert("Eroare", "Câmpurile Nume și Specie sunt obligatorii!", "OK");
            return;
        }

        // 2. ACTUALIZAM OBIECTUL CU CE A SCRIS UTILIZATORUL
        _petToEdit.Nume = TxtNume.Text;
        _petToEdit.Specie = TxtSpecie.Text;
        _petToEdit.Rasa = TxtRasa.Text;
        _petToEdit.Gen = TxtGen.Text;

        // --- AICI AM FACUT CORECTURA ---
        // Convertim textul de la varsta in numar INTREG (int)
        if (int.TryParse(TxtVarsta.Text, out int v))
        {
            _petToEdit.Varsta = v;
        }
        else
        {
            // Daca a scris prostii la varsta (ex: "cinci"), punem 0
            _petToEdit.Varsta = 0;
        }

        // 3. SALVAM IN BAZA DE DATE
        await _dbService.UpdatePet(_petToEdit);

        // Afisam mesaj de succes
        await DisplayAlert("Succes", "Datele animalului au fost actualizate!", "OK");

        // Ne intoarcem la pagina anterioara (Dashboard)
        await Navigation.PopAsync();
    }

    // Cand apesi pe Anuleaza
    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}