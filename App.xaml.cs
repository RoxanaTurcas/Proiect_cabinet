using Proiect.Services;
using Proiect.Views; 

namespace Proiect;

public partial class App : Application
{
    public App(DatabaseService dbService)
    {
        InitializeComponent();

        // Setam pagina de start sa fie LoginPage, invelita intr-un NavigationPage
        // Asta ne permite sa navigam inainte si inapoi
        MainPage = new NavigationPage(new LoginPage(dbService));
    }
}




