namespace GymApp.ConsoleUI
{
    /// <summary>
    /// Помощни методи за форматиране на конзолния интерфейс.
    /// </summary>
    public static class ConsoleHelper
    {
        /// <summary>Цвят за заглавия.</summary>
        private const ConsoleColor TitleColor = ConsoleColor.Cyan;

        /// <summary>Цвят за успешни съобщения.</summary>
        private const ConsoleColor SuccessColor = ConsoleColor.Green;

        /// <summary>Цвят за грешки.</summary>
        private const ConsoleColor ErrorColor = ConsoleColor.Red;

        /// <summary>Цвят за предупреждения.</summary>
        private const ConsoleColor WarnColor = ConsoleColor.Yellow;

        /// <summary>Извежда заглавие с декоративна рамка.</summary>
        public static void PrintTitle(string title)
        {
            string border = new string('═', title.Length + 4);
            Console.ForegroundColor = TitleColor;
            Console.WriteLine($"╔{border}╗");
            Console.WriteLine($"║  {title}  ║");
            Console.WriteLine($"╚{border}╝");
            Console.ResetColor();
        }

        /// <summary>Извежда секционен разделител.</summary>
        public static void PrintSeparator()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(new string('─', 50));
            Console.ResetColor();
        }

        /// <summary>Извежда успешно съобщение.</summary>
        public static void PrintSuccess(string message)
        {
            Console.ForegroundColor = SuccessColor;
            Console.WriteLine($"✔  {message}");
            Console.ResetColor();
        }

        /// <summary>Извежда съобщение за грешка.</summary>
        public static void PrintError(string message)
        {
            Console.ForegroundColor = ErrorColor;
            Console.WriteLine($"✘  Грешка: {message}");
            Console.ResetColor();
        }

        /// <summary>Извежда предупреждение.</summary>
        public static void PrintWarning(string message)
        {
            Console.ForegroundColor = WarnColor;
            Console.WriteLine($"⚠  {message}");
            Console.ResetColor();
        }

        /// <summary>Извежда меню опция.</summary>
        public static void PrintMenuOption(int number, string text)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"  [{number}] ");
            Console.ResetColor();
            Console.WriteLine(text);
        }

        /// <summary>Чете непразен низ от конзолата.</summary>
        public static string ReadNonEmptyString(string prompt)
        {
            string? value;
            do
            {
                Console.Write($"  {prompt}: ");
                value = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(value))
                    PrintWarning("Полето не може да е празно. Опитайте отново.");
            }
            while (string.IsNullOrWhiteSpace(value));
            return value.Trim();
        }

        /// <summary>Чете цяло число от конзолата.</summary>
        public static int ReadInt(string prompt)
        {
            int result;
            while (true)
            {
                Console.Write($"  {prompt}: ");
                string? input = Console.ReadLine();
                if (int.TryParse(input, out result)) return result;
                PrintWarning("Въведете валидно цяло число.");
            }
        }

        /// <summary>Чете десетично число от конзолата.</summary>
        public static decimal ReadDecimal(string prompt)
        {
            decimal result;
            while (true)
            {
                Console.Write($"  {prompt}: ");
                string? input = Console.ReadLine();
                if (decimal.TryParse(input, out result)) return result;
                PrintWarning("Въведете валидна сума (напр. 50.00).");
            }
        }

        /// <summary>Чете дата от конзолата (формат yyyy-MM-dd).</summary>
        public static DateOnly ReadDate(string prompt)
        {
            DateOnly result;
            while (true)
            {
                Console.Write($"  {prompt} (yyyy-MM-dd): ");
                string? input = Console.ReadLine();
                if (DateOnly.TryParse(input, out result)) return result;
                PrintWarning("Въведете дата в правилен формат (напр. 2025-01-31).");
            }
        }

        /// <summary>Пауза — изчаква натискане на клавиш.</summary>
        public static void Pause()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\n  Натиснете Enter за продължение...");
            Console.ResetColor();
            Console.ReadLine();
        }
    }
}
