using Proiect.Models;
using Proiect.Services;

namespace Proiect.Views;

public partial class RegisterPage : ContentPage
{
    private readonly DatabaseService _dbService;

    // Primim serviciul prin constructor
    public RegisterPage(DatabaseService dbService)
    {
        InitializeComponent();
        _dbService = dbService;

        // Setam "Client" ca valoare implicita in Picker, sa fie mai usor
        RolPicker.SelectedIndex = 0;
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        // 1. Validare simpla
        if (string.IsNullOrWhiteSpace(NumeEntry.Text) ||
            string.IsNullOrWhiteSpace(EmailEntry.Text) ||
            string.IsNullOrWhiteSpace(PasswordEntry.Text))
        {
            await DisplayAlert("Eroare", "Te rog completează toate câmpurile obligatorii!", "OK");
            return;
        }

        // 2. Creare obiect User
        var newUser = new User
        {
            Nume = NumeEntry.Text,
            Prenume = PrenumeEntry.Text,
            Email = EmailEntry.Text,
            Telefon = TelefonEntry.Text,
            Parola = PasswordEntry.Text,
            Rol = RolPicker.SelectedItem.ToString() // "Client" sau "Medic"
        };

        // 3. Salvare in Baza de Date
        try
        {
            await _dbService.RegisterUser(newUser);
            await DisplayAlert("Succes", "Contul a fost creat! Te poți autentifica.", "OK");

            // Navigam inapoi la Login (scoatem pagina curenta de pe stiva)
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            // Prindem eroarea daca emailul exista deja (pentru ca am pus [Unique])
            await DisplayAlert("Eroare", "Nu s-a putut crea contul. Posibil email deja existent.", "OK");
        }
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        // Ne intoarcem la Login fara sa salvam nimic
        await Navigation.PopAsync();
    }
}