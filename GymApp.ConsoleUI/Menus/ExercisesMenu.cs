using GymApp.Data.Models;
using GymApp.Services.Interfaces;

namespace GymApp.ConsoleUI.Menus
{
    /// <summary>
    /// Конзолно меню за управление на упражнения.
    /// </summary>
    public class ExercisesMenu
    {
        private readonly IExerciseService _exerciseService;

        /// <summary>Инициализира нова инстанция на <see cref="ExercisesMenu"/>.</summary>
        public ExercisesMenu(IExerciseService exerciseService)
        {
            _exerciseService = exerciseService;
        }

        /// <summary>Показва главното меню за упражнения.</summary>
        public async Task ShowAsync()
        {
            bool running = true;
            while (running)
            {
                Console.Clear();
                ConsoleHelper.PrintTitle("УПРАВЛЕНИЕ НА УПРАЖНЕНИЯ");
                Console.WriteLine();
                ConsoleHelper.PrintMenuOption(1, "Всички упражнения");
                ConsoleHelper.PrintMenuOption(2, "Търсене по ID");
                ConsoleHelper.PrintMenuOption(3, "Филтриране по мускулна група");
                ConsoleHelper.PrintMenuOption(4, "Добавяне на упражнение");
                ConsoleHelper.PrintMenuOption(5, "Редактиране на упражнение");
                ConsoleHelper.PrintMenuOption(6, "Изтриване на упражнение");
                ConsoleHelper.PrintMenuOption(0, "Обратно");
                Console.WriteLine();
                Console.Write("  Избор: ");
                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": await ListAllAsync(); break;
                    case "2": await FindByIdAsync(); break;
                    case "3": await FilterByMuscleGroupAsync(); break;
                    case "4": await AddAsync(); break;
                    case "5": await UpdateAsync(); break;
                    case "6": await DeleteAsync(); break;
                    case "0": running = false; break;
                    default: ConsoleHelper.PrintWarning("Невалиден избор."); ConsoleHelper.Pause(); break;
                }
            }
        }

        private async Task ListAllAsync()
        {
            Console.Clear();
            ConsoleHelper.PrintTitle("ВСИЧКИ УПРАЖНЕНИЯ");
            var exercises = await _exerciseService.GetAllAsync();
            PrintTable(exercises);
            ConsoleHelper.Pause();
        }

        private async Task FindByIdAsync()
        {
            Console.Clear();
            ConsoleHelper.PrintTitle("ТЪРСЕНЕ ПО ID");
            int id = ConsoleHelper.ReadInt("ID");
            var e = await _exerciseService.GetByIdAsync(id);
            if (e == null) ConsoleHelper.PrintWarning("Не е намерено.");
            else
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"  ID: {e.ExerciseId}  Название: {e.Name}  Мускули: {e.MuscleGroup}  Оборудване: {e.Equipment}");
                Console.ResetColor();
            }
            ConsoleHelper.Pause();
        }

        private async Task FilterByMuscleGroupAsync()
        {
            Console.Clear();
            ConsoleHelper.PrintTitle("ФИЛТРИРАНЕ ПО МУСКУЛНА ГРУПА");
            string group = ConsoleHelper.ReadNonEmptyString("Мускулна група (напр. Chest, Legs, Core)");
            var exercises = await _exerciseService.GetByMuscleGroupAsync(group);
            PrintTable(exercises);
            ConsoleHelper.Pause();
        }

        private async Task AddAsync()
        {
            Console.Clear();
            ConsoleHelper.PrintTitle("ДОБАВЯНЕ НА УПРАЖНЕНИЕ");
            Console.WriteLine();
            var exercise = new Exercise
            {
                Name        = ConsoleHelper.ReadNonEmptyString("Наименование"),
                MuscleGroup = ConsoleHelper.ReadNonEmptyString("Мускулна група"),
                Equipment   = ConsoleHelper.ReadNonEmptyString("Оборудване (None ако няма)"),
            };
            try
            {
                await _exerciseService.AddAsync(exercise);
                ConsoleHelper.PrintSuccess($"Упражнението '{exercise.Name}' е добавено!");
            }
            catch (Exception ex) { ConsoleHelper.PrintError(ex.Message); }
            ConsoleHelper.Pause();
        }

        private async Task UpdateAsync()
        {
            Console.Clear();
            ConsoleHelper.PrintTitle("РЕДАКТИРАНЕ НА УПРАЖНЕНИЕ");
            int id = ConsoleHelper.ReadInt("ID");
            var exercise = await _exerciseService.GetByIdAsync(id);
            if (exercise == null) { ConsoleHelper.PrintWarning("Не е намерено."); ConsoleHelper.Pause(); return; }

            exercise.Name        = ConsoleHelper.ReadNonEmptyString($"Наименование [{exercise.Name}]");
            exercise.MuscleGroup = ConsoleHelper.ReadNonEmptyString($"Мускулна група [{exercise.MuscleGroup}]");
            exercise.Equipment   = ConsoleHelper.ReadNonEmptyString($"Оборудване [{exercise.Equipment}]");

            try
            {
                await _exerciseService.UpdateAsync(exercise);
                ConsoleHelper.PrintSuccess("Актуализирано успешно!");
            }
            catch (Exception ex) { ConsoleHelper.PrintError(ex.Message); }
            ConsoleHelper.Pause();
        }

        private async Task DeleteAsync()
        {
            Console.Clear();
            ConsoleHelper.PrintTitle("ИЗТРИВАНЕ НА УПРАЖНЕНИЕ");
            int id = ConsoleHelper.ReadInt("ID");
            Console.Write("  Потвърждавате ли? (да/не): ");
            if (Console.ReadLine()?.ToLower() != "да") { ConsoleHelper.PrintWarning("Отменено."); ConsoleHelper.Pause(); return; }
            try
            {
                await _exerciseService.DeleteAsync(id);
                ConsoleHelper.PrintSuccess("Изтрито успешно!");
            }
            catch (Exception ex) { ConsoleHelper.PrintError(ex.Message); }
            ConsoleHelper.Pause();
        }

        private static void PrintTable(IEnumerable<Exercise> exercises)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine($"  {"ID",-4} {"Наименование",-20} {"Мускулна група",-18} {"Оборудване",-15}");
            ConsoleHelper.PrintSeparator();
            Console.ResetColor();
            foreach (var e in exercises)
                Console.WriteLine($"  {e.ExerciseId,-4} {e.Name,-20} {e.MuscleGroup,-18} {e.Equipment,-15}");
        }
    }
}
