using GymApp.Data.Models;
using GymApp.Services.Interfaces;

namespace GymApp.ConsoleUI.Menus
{
    /// <summary>
    /// Конзолно меню за управление на членове.
    /// </summary>
    public class MembersMenu
    {
        private readonly IMemberService _memberService;

        /// <summary>Инициализира нова инстанция на <see cref="MembersMenu"/>.</summary>
        public MembersMenu(IMemberService memberService)
        {
            _memberService = memberService;
        }

        /// <summary>Показва главното меню за членове.</summary>
        public async Task ShowAsync()
        {
            bool running = true;
            while (running)
            {
                Console.Clear();
                ConsoleHelper.PrintTitle("УПРАВЛЕНИЕ НА ЧЛЕНОВЕ");
                Console.WriteLine();
                ConsoleHelper.PrintMenuOption(1, "Всички членове");
                ConsoleHelper.PrintMenuOption(2, "Търсене по ID");
                ConsoleHelper.PrintMenuOption(3, "Добавяне на нов член");
                ConsoleHelper.PrintMenuOption(4, "Редактиране на член");
                ConsoleHelper.PrintMenuOption(5, "Изтриване на член");
                ConsoleHelper.PrintMenuOption(6, "Активни членове (с абонамент)");
                ConsoleHelper.PrintMenuOption(7, "Членове с тренировки");
                ConsoleHelper.PrintMenuOption(0, "Обратно към главното меню");
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
                    case "6": await ListActiveAsync(); break;
                    case "7": await ListWithWorkoutsAsync(); break;
                    case "0": running = false; break;
                    default: ConsoleHelper.PrintWarning("Невалиден избор."); ConsoleHelper.Pause(); break;
                }
            }
        }

        private async Task ListAllAsync()
        {
            Console.Clear();
            ConsoleHelper.PrintTitle("ВСИЧКИ ЧЛЕНОВЕ");
            var members = await _memberService.GetAllAsync();
            PrintMemberTable(members);
            ConsoleHelper.Pause();
        }

        private async Task FindByIdAsync()
        {
            Console.Clear();
            ConsoleHelper.PrintTitle("ТЪРСЕНЕ НА ЧЛЕН ПО ID");
            int id = ConsoleHelper.ReadInt("Въведете ID");
            var member = await _memberService.GetByIdAsync(id);
            if (member == null)
            {
                ConsoleHelper.PrintWarning("Членът не е намерен.");
            }
            else
            {
                Console.WriteLine();
                PrintMemberDetails(member);
            }
            ConsoleHelper.Pause();
        }

        private async Task AddAsync()
        {
            Console.Clear();
            ConsoleHelper.PrintTitle("ДОБАВЯНЕ НА НОВ ЧЛЕН");
            Console.WriteLine();
            var member = new Member
            {
                FirstName = ConsoleHelper.ReadNonEmptyString("Собствено име"),
                LastName  = ConsoleHelper.ReadNonEmptyString("Фамилно име"),
                Gender    = ConsoleHelper.ReadNonEmptyString("Пол (Male/Female)"),
                BirthDate = ConsoleHelper.ReadDate("Дата на раждане"),
            };
            Console.Write("  Телефон (оставете празно за пропускане): ");
            string? phone = Console.ReadLine();
            if (long.TryParse(phone, out long phoneNum))
                member.PhoneNumber = phoneNum;

            try
            {
                await _memberService.AddAsync(member);
                ConsoleHelper.PrintSuccess($"Членът {member.FirstName} {member.LastName} е добавен успешно!");
            }
            catch (Exception ex)
            {
                ConsoleHelper.PrintError(ex.Message);
            }
            ConsoleHelper.Pause();
        }

        private async Task UpdateAsync()
        {
            Console.Clear();
            ConsoleHelper.PrintTitle("РЕДАКТИРАНЕ НА ЧЛЕН");
            int id = ConsoleHelper.ReadInt("Въведете ID на члена");
            var member = await _memberService.GetByIdAsync(id);
            if (member == null)
            {
                ConsoleHelper.PrintWarning("Членът не е намерен.");
                ConsoleHelper.Pause();
                return;
            }

            Console.WriteLine($"\n  Текущо: {member.FirstName} {member.LastName} | {member.Gender} | {member.BirthDate}");
            Console.WriteLine("  (Оставете празно за запазване на текущата стойност)\n");

            string first = ConsoleHelper.ReadNonEmptyString($"Собствено име [{member.FirstName}]");
            string last  = ConsoleHelper.ReadNonEmptyString($"Фамилно [{member.LastName}]");
            string gen   = ConsoleHelper.ReadNonEmptyString($"Пол [{member.Gender}]");

            member.FirstName = string.IsNullOrWhiteSpace(first) ? member.FirstName : first;
            member.LastName  = string.IsNullOrWhiteSpace(last)  ? member.LastName  : last;
            member.Gender    = string.IsNullOrWhiteSpace(gen)   ? member.Gender    : gen;

            try
            {
                await _memberService.UpdateAsync(member);
                ConsoleHelper.PrintSuccess("Данните са актуализирани успешно!");
            }
            catch (Exception ex)
            {
                ConsoleHelper.PrintError(ex.Message);
            }
            ConsoleHelper.Pause();
        }

        private async Task DeleteAsync()
        {
            Console.Clear();
            ConsoleHelper.PrintTitle("ИЗТРИВАНЕ НА ЧЛЕН");
            int id = ConsoleHelper.ReadInt("Въведете ID на члена за изтриване");
            Console.Write("  Потвърждавате ли изтриването? (да/не): ");
            string? confirm = Console.ReadLine();
            if (confirm?.ToLower() != "да")
            {
                ConsoleHelper.PrintWarning("Операцията е отменена.");
                ConsoleHelper.Pause();
                return;
            }
            try
            {
                await _memberService.DeleteAsync(id);
                ConsoleHelper.PrintSuccess("Членът е изтрит успешно!");
            }
            catch (Exception ex)
            {
                ConsoleHelper.PrintError(ex.Message);
            }
            ConsoleHelper.Pause();
        }

        private async Task ListActiveAsync()
        {
            Console.Clear();
            ConsoleHelper.PrintTitle("АКТИВНИ ЧЛЕНОВЕ");
            var members = await _memberService.GetActiveMembersAsync();
            PrintMemberTable(members);
            ConsoleHelper.Pause();
        }

        private async Task ListWithWorkoutsAsync()
        {
            Console.Clear();
            ConsoleHelper.PrintTitle("ЧЛЕНОВЕ С ТРЕНИРОВКИ");
            var members = await _memberService.GetMembersWithWorkoutsAsync();
            foreach (var m in members)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"\n  ► {m.FirstName} {m.LastName}");
                Console.ResetColor();
                if (!m.MemberWorkouts.Any())
                {
                    ConsoleHelper.PrintWarning("    Няма записани тренировки.");
                }
                else
                {
                    foreach (var mw in m.MemberWorkouts)
                        Console.WriteLine($"    • {mw.Workout.Name}  ({mw.StartHour} – {mw.EndHour})");
                }
            }
            ConsoleHelper.Pause();
        }

        // ── helpers ────────────────────────────────────────────────────────────

        private static void PrintMemberTable(IEnumerable<Member> members)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine($"  {"ID",-4} {"Собствено",-14} {"Фамилно",-14} {"Пол",-8} {"Рожден ден",-12} {"Телефон",-12}");
            ConsoleHelper.PrintSeparator();
            Console.ResetColor();
            foreach (var m in members)
                Console.WriteLine($"  {m.MembersId,-4} {m.FirstName,-14} {m.LastName,-14} {m.Gender,-8} {m.BirthDate,-12} {m.PhoneNumber,-12}");
        }

        private static void PrintMemberDetails(Member m)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"  ID:         {m.MembersId}");
            Console.WriteLine($"  Име:        {m.FirstName} {m.LastName}");
            Console.WriteLine($"  Пол:        {m.Gender}");
            Console.WriteLine($"  Рождена:    {m.BirthDate}");
            Console.WriteLine($"  Телефон:    {m.PhoneNumber}");
            Console.ResetColor();
        }
    }
}
