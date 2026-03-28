using GymApp.ConsoleUI;
using GymApp.ConsoleUI.Menus;
using GymApp.Data.Context;
using GymApp.Services.Implementations;
using GymApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.Metrics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

// ── Dependency Injection setup ─────────────────────────────────────────────
var services = new ServiceCollection();

// ВАЖНО: Заменете connection string-а с вашите данни!
// Формат: "Server=localhost;Database=gym;User=root;Password=вашата_парола;"
string connectionString = "Server=localhost;Database=gym;User=root;Password=1234567890;";

services.AddDbContext<GymDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

services.AddScoped<IMemberService,      MemberService>();
services.AddScoped<ITrainerService,     TrainerService>();
services.AddScoped<IWorkoutService,     WorkoutService>();
services.AddScoped<IExerciseService,    ExerciseService>();
services.AddScoped<ISubscriptionService, SubscriptionService>();

services.AddScoped<MembersMenu>();
services.AddScoped<TrainersMenu>();
services.AddScoped<WorkoutsMenu>();
services.AddScoped<ExercisesMenu>();
services.AddScoped<SubscriptionsMenu>();

var provider = services.BuildServiceProvider();

// ── Main loop ──────────────────────────────────────────────────────────────
bool appRunning = true;

while (appRunning)
{
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.DarkYellow;
    Console.WriteLine(@"
  ███╗   ███╗ █████╗ ███████╗████████╗███████╗██████╗      ██████╗ ██╗   ██╗███╗   ███╗
  ████╗ ████║██╔══██╗██╔════╝╚══██╔══╝██╔════╝██╔══██╗    ██╔════╝ ╚██╗ ██╔╝████╗ ████║
  ██╔████╔██║███████║███████╗   ██║   █████╗  ██████╔╝    ██║  ███╗ ╚████╔╝ ██╔████╔██║
  ██║╚██╔╝██║██╔══██║╚════██║   ██║   ██╔══╝  ██╔══██╗    ██║   ██║  ╚██╔╝  ██║╚██╔╝██║
  ██║ ╚═╝ ██║██║  ██║███████║   ██║   ███████╗██║  ██║    ╚██████╔╝   ██║   ██║ ╚═╝ ██║
  ╚═╝     ╚═╝╚═╝  ╚═╝╚══════╝   ╚═╝   ╚══════╝╚═╝  ╚═╝     ╚═════╝    ╚═╝   ╚═╝     ╚═╝
    ");
    Console.ResetColor();

    ConsoleHelper.PrintTitle("ГЛАВНО МЕНЮ");
    Console.WriteLine();
    ConsoleHelper.PrintMenuOption(1, "Членове");
    ConsoleHelper.PrintMenuOption(2, "Треньори");
    ConsoleHelper.PrintMenuOption(3, "Тренировки");
    ConsoleHelper.PrintMenuOption(4, "Упражнения");
    ConsoleHelper.PrintMenuOption(5, "Абонаменти");
    ConsoleHelper.PrintMenuOption(0, "Изход");
    Console.WriteLine();
    Console.Write("  Избор: ");
    string? choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            await provider.GetRequiredService<MembersMenu>().ShowAsync();
            break;
        case "2":
            await provider.GetRequiredService<TrainersMenu>().ShowAsync();
            break;
        case "3":
            await provider.GetRequiredService<WorkoutsMenu>().ShowAsync();
            break;
        case "4":
            await provider.GetRequiredService<ExercisesMenu>().ShowAsync();
            break;
        case "5":
            await provider.GetRequiredService<SubscriptionsMenu>().ShowAsync();
            break;
        case "0":
            appRunning = false;
            Console.Clear();
            ConsoleHelper.PrintTitle("Благодарим, че използвате Master GYM!");
            Console.WriteLine();
            break;
        default:
            ConsoleHelper.PrintWarning("Невалиден избор.");
            ConsoleHelper.Pause();
            break;
    }
}
