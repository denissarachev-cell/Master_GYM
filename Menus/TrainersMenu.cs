using GymApp.Data.Models;
using GymApp.Services.Interfaces;

namespace GymApp.ConsoleUI.Menus
{
    /// <summary>
    /// Конзолно меню за управление на треньори.
    /// </summary>
    public class TrainersMenu
    {
        private readonly ITrainerService _trainerService;

        /// <summary>Инициализира нова инстанция на <see cref="TrainersMenu"/>.</summary>
        public TrainersMenu(ITrainerService trainerService)
        {
            _trainerService = trainerService;
        }

        /// <summary>Показва главното меню за треньори.</summary>
        public async Task ShowAsync()
        {
            bool running = true;
            while (running)
            {
                Console.Clear();
                ConsoleHelper.PrintTitle("УПРАВЛЕНИЕ НА ТРЕНЬОРИ");
                Console.WriteLine();
                ConsoleHelper.PrintMenuOption(1, "Всички треньори");
                ConsoleHelper.PrintMenuOption(2, "Търсене по ID");
                ConsoleHelper.PrintMenuOption(3, "Добавяне на нов треньор");
                ConsoleHelper.PrintMenuOption(4, "Редактиране на треньор");
                ConsoleHelper.PrintMenuOption(5, "Изтриване на треньор");
                ConsoleHelper.PrintMenuOption(6, "Треньори с тренировки");
                ConsoleHelper.PrintMenuOption(0, "Обратно");
                Console.WriteLine();
                Console.Write("  Избор: ");
                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": await ListAllAsync(); break;
                    case "2": await FindByIdAsync(); break;
                    case "3": await AddAsync(); break;
                    case "4": await UpdateAsync(); break;
                    case "5": await DeleteAsync(); break;
                    case "6": await ListWithWorkoutsAsync(); break;
                    case "0": running = false; break;
                    default: ConsoleHelper.PrintWarning("Невалиден избор."); ConsoleHelper.Pause(); break;
                }
            }
        }

        private async Task ListAllAsync()
        {
            Console.Clear();
            ConsoleHelper.PrintTitle("ВСИЧКИ ТРЕНЬОРИ");
            var trainers = await _trainerService.GetAllAsync();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine($"  {"ID",-4} {"Собствено",-14} {"Фамилно",-14} {"Телефон",-12}");
            ConsoleHelper.PrintSeparator();
            Console.ResetColor();
            foreach (var t in trainers)
                Console.WriteLine($"  {t.TrainerId,-4} {t.FirstName,-14} {t.LastName,-14} {t.PhoneNumber,-12}");
            ConsoleHelper.Pause();
        }

        private async Task FindByIdAsync()
        {
            Console.Clear();
            ConsoleHelper.PrintTitle("ТЪРСЕНЕ НА ТРЕНЬОР ПО ID");
            int id = ConsoleHelper.ReadInt("Въведете ID");
            var trainer = await _trainerService.GetByIdAsync(id);
            if (trainer == null)
                ConsoleHelper.PrintWarning("Треньорът не е намерен.");
            else
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"  ID: {trainer.TrainerId}  Ime: {trainer.FirstName} {trainer.LastName}  Tel: {trainer.PhoneNumber}");
                Console.ResetColor();
            }
            ConsoleHelper.Pause();
        }

        private async Task AddAsync()
        {
            Console.Clear();
            ConsoleHelper.PrintTitle("ДОБАВЯНЕ НА НОВ ТРЕНЬОР");
            Console.WriteLine();
            var trainer = new Trainer
            {
                FirstName = ConsoleHelper.ReadNonEmptyString("Собствено име"),
                LastName  = ConsoleHelper.ReadNonEmptyString("Фамилно име"),
            };
            Console.Write("  Телефон (оставете празно за пропускане): ");
            if (long.TryParse(Console.ReadLine(), out long phone))
                trainer.PhoneNumber = phone;

            try
            {
                await _trainerService.AddAsync(trainer);
                ConsoleHelper.PrintSuccess($"Треньорът {trainer.FirstName} {trainer.LastName} е добавен успешно!");
            }
            catch (Exception ex) { ConsoleHelper.PrintError(ex.Message); }
            ConsoleHelper.Pause();
        }

        private async Task UpdateAsync()
        {
            Console.Clear();
            ConsoleHelper.PrintTitle("РЕДАКТИРАНЕ НА ТРЕНЬОР");
            int id = ConsoleHelper.ReadInt("Въведете ID");
            var trainer = await _trainerService.GetByIdAsync(id);
            if (trainer == null) { ConsoleHelper.PrintWarning("Не е намерен."); ConsoleHelper.Pause(); return; }

            trainer.FirstName = ConsoleHelper.ReadNonEmptyString($"Собствено [{trainer.FirstName}]");
            trainer.LastName  = ConsoleHelper.ReadNonEmptyString($"Фамилно [{trainer.LastName}]");

            try
            {
                await _trainerService.UpdateAsync(trainer);
                ConsoleHelper.PrintSuccess("Данните са актуализирани!");
            }
            catch (Exception ex) { ConsoleHelper.PrintError(ex.Message); }
            ConsoleHelper.Pause();
        }

        private async Task DeleteAsync()
        {
            Console.Clear();
            ConsoleHelper.PrintTitle("ИЗТРИВАНЕ НА ТРЕНЬОР");
            int id = ConsoleHelper.ReadInt("Въведете ID");
            Console.Write("  Потвърждавате ли? (да/не): ");
            if (Console.ReadLine()?.ToLower() != "да")
            {
                ConsoleHelper.PrintWarning("Отменено."); ConsoleHelper.Pause(); return;
            }
            try
            {
                await _trainerService.DeleteAsync(id);
                ConsoleHelper.PrintSuccess("Треньорът е изтрит!");
            }
            catch (Exception ex) { ConsoleHelper.PrintError(ex.Message); }
            ConsoleHelper.Pause();
        }

        private async Task ListWithWorkoutsAsync()
        {
            Console.Clear();
            ConsoleHelper.PrintTitle("ТРЕНЬОРИ С ТРЕНИРОВКИ");
            var trainers = await _trainerService.GetTrainersWithWorkoutsAsync();
            foreach (var t in trainers)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"\n  ► {t.FirstName} {t.LastName}");
                Console.ResetColor();
                if (!t.Workouts.Any())
                    Console.WriteLine("    (няма тренировки)");
                else
                    foreach (var w in t.Workouts)
                        Console.WriteLine($"    • {w.Name}  [{w.DifficultyLevel}]");
            }
            ConsoleHelper.Pause();
        }
    }
}
