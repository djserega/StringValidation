namespace StringValidation
{
    /// <summary>
    /// Читання даних з файлу. Перевірка відповідності даних до шаблону валідації
    /// </summary>
    internal class Reader
    {
        /// <summary>
        /// Базовий конструктор
        /// </summary>
        /// <param name="fileName">Назва файлу з даними які потрібно перевірити</param>
        public Reader(string fileName)
        {
            FileName = fileName;
        }

        /// <summary>
        /// Назва файлу з даними які потрібно перевірити
        /// </summary>
        internal string FileName { get; init; }

        /// <summary>
        /// Читання та попередня валідація даних
        /// </summary>
        /// <returns>Список рядків файлу які пройшли перевірку на відповідність до шаблону валідації</returns>
        internal async Task<List<Models.Row>> ReadData()
        {
            Console.ForegroundColor = ConsoleColor.DarkBlue;

            string?[]? data = await ReadInputFileAsync();

            if (data == null)
            {
                return new List<Models.Row>();
            }
            else
            {
                ValidationPattern pattern = new();

                return pattern.ValidateData(data);
            }
        }

        /// <summary>
        /// Префікс статусу читання рядків
        /// </summary>
        readonly string _prefixInfo = "Читання файлу. Прочитано рядків: ";

        /// <summary>
        /// Читання даних з файлу
        /// </summary>
        /// <returns>Масив прочитаних даних</returns>
        private async Task<string?[]?> ReadInputFileAsync()
        {
            FileInfo file = new(FileName);

            if (!file.Exists)
            {
                WriteError($"Не знайдено файл: {FileName}");
                return default;
            }

            List<string?> fileRows = new();

            int numRows = 0;
            Console.Write($"\n{_prefixInfo}{numRows}");

            try
            {
                using StreamReader reader = file.OpenText();
                {
                    while (!reader.EndOfStream)
                    {
                        fileRows.Add(await reader.ReadLineAsync());
                        Console.Write($"\r{_prefixInfo}{++numRows}");
                    }
                }
            }
            catch (Exception ex)
            {
                WriteError($"Помилка читання файлу: {FileName}\n{ex.Message}");
                return default;
            }

            Console.WriteLine("\nФайл прочитано\n");

            return fileRows.ToArray();
        }

        /// <summary>
        /// Вивід інформації з помилкою. Інформація виділяється іншим кольором
        /// </summary>
        /// <param name="message">Текст повідомлення</param>
        private static void WriteError(string message)
        {
            ConsoleColor currentColor = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ForegroundColor = currentColor;
        }
    }
}
