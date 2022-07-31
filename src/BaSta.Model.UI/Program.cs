using BaSta.Model;
using BaSta.Model.UI.Resources;
using BaSta.Model.UI.ViewModel;
using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

IGame game = new Game();
game.Home.SetName("Medipolis SC Jena");
game.Home.SetNameInitials("SJC");
game.Home.SetNameShort("Jena");
game.Home.SetLogo(Resource.Medipolis_SC_Jena);
game.Home.AddPlayer(new Player("Wolf", 10));
game.Home.AddPlayer(new Player("Haukohl", 35));
game.Home.AddPlayer(new Player("Herrera", 41));
game.Home.AddPlayer(new Player("Lacy", 1));
game.Home.AddPlayer(new Player("Plescher", 16));
game.Home.AddPlayer(new Player("Simmons", 11));
game.Home.AddPlayer(new Player("Radojicic", 44));
game.Home.AddPlayer(new Player("Brauner", 5));
game.Home.AddPlayer(new Player("Bank", 25));
game.Home.AddPlayer(new Player("Alberton", 21));
game.Home.AddPlayer(new Player("Thomas", 33));
game.Home.AddPlayer(new Player("Chapman", 13));

game.Guest.SetName("VfL SparkassenStars Bochum");
game.Guest.SetLogo(Resource.VfL_SparkassenStars_Bochum);
game.Guest.AddPlayer(new Player("Geske", 5));
game.Guest.AddPlayer(new Player("Kamp", 7));
game.Guest.AddPlayer(new Player("Joos", 22));
game.Guest.AddPlayer(new Player("Rohwer", 27));
game.Guest.AddPlayer(new Player("Dietz", 8));
game.Guest.AddPlayer(new Player("Hicks", 11));

builder.Services.AddRazorPages(options =>
{
    options.RootDirectory = "/";
});
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton(new MainViewModel(game));

builder.Services.AddBlazorise(options =>
    {
        options.Immediate = true;
    })
    .AddBootstrap5Providers()
    .AddFontAwesomeIcons();

WebApplication app = builder.Build();

app.UseStaticFiles();
app.MapFallbackToPage("/Index");

app.MapBlazorHub();

app.Run();