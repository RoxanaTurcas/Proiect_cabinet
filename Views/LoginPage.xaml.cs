using Proiect.Services;
using Proiect.Models;

namespace Proiect.Views;

public partial class LoginPage : ContentPage
{
    private readonly DatabaseService _dbService;

    // Injectam serviciul de baza de date in constructor
    public LoginPage(DatabaseService dbService)
    {
        InitializeComponent();
        _dbService = dbService;
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        string email = EmailEntry.Text;
        string parola = PasswordEntry.Text;

        // Validare simpla
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(parola))
        {
            await DisplayAlert("Eroare", "Te rog completează toate câmpurile.", "OK");
            return;
        }

        // Verificam in baza de date
        var user = await _dbService.Login(email, parola);

        if (user != null)
        {
            // Succes! Navigam la Dashboard si trimitem userul gasit
            await Navigation.PushAsync(new DashboardPage(_dbService, user));
        }
        else
        {
            await DisplayAlert("Eroare", "Email sau parolă incorectă.", "OK");
        }
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        // Navigam catre pagina de Register si trimitem dbService mai departe
        await Navigation.PushAsync(new RegisterPage(_dbService));
    }
}