using GymApp.Data.Models;
using GymApp.Services.Interfaces;

namespace GymApp.ConsoleUI.Menus
{
    /// <summary>
    /// Конзолно меню за управление на абонаменти.
    /// </summary>
    public class SubscriptionsMenu
    {
        private readonly ISubscriptionService _subscriptionService;
        private readonly IMemberService _memberService;

        /// <summary>Инициализира нова инстанция на <see cref="SubscriptionsMenu"/>.</summary>
        public SubscriptionsMenu(ISubscriptionService subscriptionService, IMemberService memberService)
        {
            _subscriptionService = subscriptionService;
            _memberService = memberService;
        }

        /// <summary>Показва главното меню за абонаменти.</summary>
        public async Task ShowAsync()
        {
            bool running = true;
            while (running)
            {
                Console.Clear();
                ConsoleHelper.PrintTitle("УПРАВЛЕНИЕ НА АБОНАМЕНТИ");
                Console.WriteLine();
                ConsoleHelper.PrintMenuOption(1, "Всички абонаменти");
                ConsoleHelper.PrintMenuOption(2, "Активни абонаменти (към днес)");
                ConsoleHelper.PrintMenuOption(3, "Абонаменти по член (ID)");
                ConsoleHelper.PrintMenuOption(4, "Добавяне на абонамент");
                ConsoleHelper.PrintMenuOption(5, "Изтриване на абонамент");
                ConsoleHelper.PrintMenuOption(0, "Обратно");
                Console.WriteLine();
                Console.Write("  Избор: ");
                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": await ListAllAsync(); break;
                    case "2": await ListActiveAsync(); break;
                    case "3": await ListByMemberAsync(); break;
                    case "4": await AddAsync(); break;
                    case "5": await DeleteAsync(); break;
                    case "0": running = false; break;
                    default: ConsoleHelper.PrintWarning("Невалиден избор."); ConsoleHelper.Pause(); break;
                }
            }
        }

        private async Task ListAllAsync()
        {
            Console.Clear();
            ConsoleHelper.PrintTitle("ВСИЧКИ АБОНАМЕНТИ");
            var subs = await _subscriptionService.GetAllAsync();
            PrintTable(subs);
            ConsoleHelper.Pause();
        }

        private async Task ListActiveAsync()
        {
            Console.Clear();
            ConsoleHelper.PrintTitle($"АКТИВНИ АБОНАМЕНТИ  ({DateTime.Today:yyyy-MM-dd})");
            var subs = await _subscriptionService.GetActiveSubscriptionsAsync();
            PrintTable(subs);
            ConsoleHelper.Pause();
        }

        private async Task ListByMemberAsync()
        {
            Console.Clear();
            ConsoleHelper.PrintTitle("АБОНАМЕНТИ ПО ЧЛЕН");
            int memberId = ConsoleHelper.ReadInt("ID на члена");
            var subs = await _subscriptionService.GetByMemberIdAsync(memberId);
            PrintTable(subs);
            ConsoleHelper.Pause();
        }

        private async Task AddAsync()
        {
            Console.Clear();
            ConsoleHelper.PrintTitle("ДОБАВЯНЕ НА АБОНАМЕНТ");
            Console.WriteLine();

            // Покажи членовете
            var members = (await _memberService.GetAllAsync()).ToList();
            Console.WriteLine("  Налични членове:");
            foreach (var m in members)
                Console.WriteLine($"    [{m.MembersId}] {m.FirstName} {m.LastName}");
            Console.WriteLine();

            var subscription = new Subscription
            {
                MemberId  = ConsoleHelper.ReadInt("ID на члена"),
                Type      = ConsoleHelper.ReadNonEmptyString("Тип (Monthly/Annual)"),
                Price     = ConsoleHelper.ReadDecimal("Цена"),
                StartDate = ConsoleHelper.ReadDate("Начална дата"),
                EndDate   = ConsoleHelper.ReadDate("Крайна дата"),
            };

            try
            {
                await _subscriptionService.AddAsync(subscription);
                ConsoleHelper.PrintSuccess("Абонаментът е добавен успешно!");
            }
            catch (Exception ex) { ConsoleHelper.PrintError(ex.Message); }
            ConsoleHelper.Pause();
        }

        private async Task DeleteAsync()
        {
            Console.Clear();
            ConsoleHelper.PrintTitle("ИЗТРИВАНЕ НА АБОНАМЕНТ");
            int id = ConsoleHelper.ReadInt("ID на абонамента");
            Console.Write("  Потвърждавате ли? (да/не): ");
            if (Console.ReadLine()?.ToLower() != "да") { ConsoleHelper.PrintWarning("Отменено."); ConsoleHelper.Pause(); return; }
            try
            {
                await _subscriptionService.DeleteAsync(id);
                ConsoleHelper.PrintSuccess("Изтрит успешно!");
            }
            catch (Exception ex) { ConsoleHelper.PrintError(ex.Message); }
            ConsoleHelper.Pause();
        }

        private static void PrintTable(IEnumerable<Subscription> subs)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine($"  {"ID",-4} {"Член",-22} {"Тип",-10} {"Цена",-8} {"От",-12} {"До",-13} {"Активен",-8}");
            ConsoleHelper.PrintSeparator();
            Console.ResetColor();
            foreach (var s in subs)
            {
                string active = s.IsActive ? "Yes" : "No";
                Console.ForegroundColor = s.IsActive ? ConsoleColor.Green : ConsoleColor.DarkGray;
                Console.WriteLine($"  {s.SubscriptionId,-4} {s.Member?.FirstName + " " + s.Member?.LastName,-22} {s.Type,-10} {s.Price,-8:F2} {s.StartDate,-12} {s.EndDate,-13} {active,-8}");
                Console.ResetColor();
            }
        }
    }
}
