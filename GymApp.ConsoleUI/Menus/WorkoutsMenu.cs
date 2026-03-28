using GymApp.Data.Models;
using GymApp.Services.Interfaces;

namespace GymApp.ConsoleUI.Menus
{
    /// <summary>
    /// Конзолно меню за управление на тренировки.
    /// </summary>
    public class WorkoutsMenu
    {
        private readonly IWorkoutService _workoutService;
        private readonly ITrainerService _trainerService;

        /// <summary>Инициализира нова инстанция на <see cref="WorkoutsMenu"/>.</summary>
        public WorkoutsMenu(IWorkoutService workoutService, ITrainerService trainerService)
        {
            _workoutService = workoutService;
            _trainerService = trainerService;
        }

        /// <summary>Показва главното меню за тренировки.</summary>
        public async Task ShowAsync()
        {
            bool running = true;
            while (running)
            {
                Console.Clear();
                ConsoleHelper.PrintTitle("УПРАВЛЕНИЕ НА ТРЕНИРОВКИ");
                Console.WriteLine();
                ConsoleHelper.PrintMenuOption(1, "Всички тренировки");
                ConsoleHelper.PrintMenuOption(2, "Търсене по ID");
                ConsoleHelper.PrintMenuOption(3, "Добавяне на тренировка");
                ConsoleHelper.PrintMenuOption(4, "Редактиране на тренировка");
                ConsoleHelper.PrintMenuOption(5, "Изтриване на тренировка");
                ConsoleHelper.PrintMenuOption(6, "Филтриране по трудност (Easy/Medium/Hard)");
                ConsoleHelper.PrintMenuOption(7, "Тренировки с упражнения");
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
                    case "6": await FilterByDifficultyAsync(); break;
                    case "7": await ListWithDetailsAsync(); break;
                    case "0": running = false; break;
                    default: ConsoleHelper.PrintWarning("Невалиден избор."); ConsoleHelper.Pause(); break;
                }
            }
        }

        private async Task ListAllAsync()
        {
            Console.Clear();
            ConsoleHelper.PrintTitle("ВСИЧКИ ТРЕНИРОВКИ");
            var workouts = await _workoutService.GetAllAsync();
            PrintWorkoutTable(workouts);
            ConsoleHelper.Pause();
        }

        private async Task FindByIdAsync()
        {
            Console.Clear();
            ConsoleHelper.PrintTitle("ТЪРСЕНЕ НА ТРЕНИРОВКА ПО ID");
            int id = ConsoleHelper.ReadInt("ID");
            var w = await _workoutService.GetByIdAsync(id);
            if (w == null) { ConsoleHelper.PrintWarning("Не е намерена."); }
            else
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"  ID: {w.WorkoutId}  Наименование: {w.Name}  Трудност: {w.DifficultyLevel}  Треньор: {w.Trainer?.FirstName} {w.Trainer?.LastName}");
                Console.ResetColor();
            }
            ConsoleHelper.Pause();
        }

        private async Task AddAsync()
        {
            Console.Clear();
            ConsoleHelper.PrintTitle("ДОБАВЯНЕ НА ТРЕНИРОВКА");
            Console.WriteLine();

            // Покажи треньорите
            var trainers = (await _trainerService.GetAllAsync()).ToList();
            Console.WriteLine("  Налични треньори:");
            foreach (var t in trainers)
                Console.WriteLine($"    [{t.TrainerId}] {t.FirstName} {t.LastName}");
            Console.WriteLine();

            var workout = new Workout
            {
                Name            = ConsoleHelper.ReadNonEmptyString("Наименование"),
                DifficultyLevel = ConsoleHelper.ReadNonEmptyString("Трудност (Easy/Medium/Hard)"),
                TrainerId       = ConsoleHelper.ReadInt("ID на треньора"),
            };

            try
            {
                await _workoutService.AddAsync(workout);
                ConsoleHelper.PrintSuccess($"Тренировката '{workout.Name}' е добавена!");
            }
            catch (Exception ex) { ConsoleHelper.PrintError(ex.Message); }
            ConsoleHelper.Pause();
        }

        private async Task UpdateAsync()
        {
            Console.Clear();
            ConsoleHelper.PrintTitle("РЕДАКТИРАНЕ НА ТРЕНИРОВКА");
            int id = ConsoleHelper.ReadInt("ID");
            var workout = await _workoutService.GetByIdAsync(id);
            if (workout == null) { ConsoleHelper.PrintWarning("Не е намерена."); ConsoleHelper.Pause(); return; }

            workout.Name            = ConsoleHelper.ReadNonEmptyString($"Наименование [{workout.Name}]");
            workout.DifficultyLevel = ConsoleHelper.ReadNonEmptyString($"Трудност [{workout.DifficultyLevel}]");

            try
            {
                await _workoutService.UpdateAsync(workout);
                ConsoleHelper.PrintSuccess("Тренировката е актуализирана!");
            }
            catch (Exception ex) { ConsoleHelper.PrintError(ex.Message); }
            ConsoleHelper.Pause();
        }

        private async Task DeleteAsync()
        {
            Console.Clear();
            ConsoleHelper.PrintTitle("ИЗТРИВАНЕ НА ТРЕНИРОВКА");
            int id = ConsoleHelper.ReadInt("ID");
            Console.Write("  Потвърждавате ли? (да/не): ");
            if (Console.ReadLine()?.ToLower() != "да") { ConsoleHelper.PrintWarning("Отменено."); ConsoleHelper.Pause(); return; }
            try
            {
                await _workoutService.DeleteAsync(id);
                ConsoleHelper.PrintSuccess("Изтрита успешно!");
            }
            catch (Exception ex) { ConsoleHelper.PrintError(ex.Message); }
            ConsoleHelper.Pause();
        }

        private async Task FilterByDifficultyAsync()
        {
            Console.Clear();
            ConsoleHelper.PrintTitle("ФИЛТРИРАНЕ ПО ТРУДНОСТ");
            string diff = ConsoleHelper.ReadNonEmptyString("Трудност (Easy/Medium/Hard)");
            var workouts = await _workoutService.GetByDifficultyAsync(diff);
            PrintWorkoutTable(workouts);
            ConsoleHelper.Pause();
        }

        private async Task ListWithDetailsAsync()
        {
            Console.Clear();
            ConsoleHelper.PrintTitle("ТРЕНИРОВКИ С УПРАЖНЕНИЯ");
            var workouts = await _workoutService.GetWorkoutsWithDetailsAsync();
            foreach (var w in workouts)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"\n  ► {w.Name}  [{w.DifficultyLevel}]  — {w.Trainer?.FirstName} {w.Trainer?.LastName}");
                Console.ResetColor();
                if (!w.WorkoutExercises.Any())
                    Console.WriteLine("    (няма упражнения)");
                else
                    foreach (var we in w.WorkoutExercises)
                        Console.WriteLine($"    • {we.Exercise.Name}  {we.Sets} серии × {we.Reps} повторения  ({we.Exercise.MuscleGroup})");
            }
            ConsoleHelper.Pause();
        }

        private static void PrintWorkoutTable(IEnumerable<Workout> workouts)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine($"  {"ID",-4} {"Наименование",-22} {"Трудност",-10} {"Треньор",-22}");
            ConsoleHelper.PrintSeparator();
            Console.ResetColor();
            foreach (var w in workouts)
                Console.WriteLine($"  {w.WorkoutId,-4} {w.Name,-22} {w.DifficultyLevel,-10} {w.Trainer?.FirstName + " " + w.Trainer?.LastName,-22}");
        }
    }
}
