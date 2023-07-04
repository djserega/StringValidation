using System.Text;


Console.OutputEncoding = Encoding.Unicode;
Console.InputEncoding = Encoding.Unicode;

ConsoleKey exitKey = default;
do
{
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Gray;

    Console.Write("Введіть шлях до файлу перевірки: ");

    string? validationFilePath = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(validationFilePath))
    {
        Console.WriteLine("Не вказано файл з даними");
    }
    else
    {
        List<StringValidation.Models.Row> validationRows = await ReadFileValidationAsync(validationFilePath);

        if (validationRows.Any())
        {
            // Обробка корректних для аналізу даних
            await ProcessingDataAsync(validationRows);
        }
    }

    Console.ForegroundColor = ConsoleColor.Gray;
    Console.Write("\nДля виходу із програми натисність 'Q', для перевірки нового файлу натисніть будь яку клавішу...");

    exitKey = Console.ReadKey(true).Key;

} while (exitKey != default && exitKey != ConsoleKey.Q);


/// <summary>
/// Читання файлу та повернення корректних рядків
/// </summary>
/// <param name="validationFilePath">Шлях до файлу перевірки</param>
/// <returns>Рядки які відповідають шаблону валідації</returns>
static async Task<List<StringValidation.Models.Row>> ReadFileValidationAsync(string validationFilePath)
{
    // Ініціалізація читання файлу
    StringValidation.Reader reader = new(validationFilePath.Trim());

    // Парсінг файлу та створення масиву даних готових для перевірки
    List<StringValidation.Models.Row> validationRows = await reader.ReadData();

    return validationRows;
}

/// <summary>
/// Обробка даних. Перевірка прочитаних рядків файлу. Розділення обробки даних на потоки 
/// </summary>
/// <param name="validationRows">Список прочитаних рядків файлу</param>
static async Task ProcessingDataAsync(List<StringValidation.Models.Row> validationRows)
{
    Console.ForegroundColor = ConsoleColor.DarkYellow;

    Console.WriteLine("Аналіз даних файлу");

    int numberOfValidRows = 0;
    int numberOfNotValidRows = 0;

    // Кількість потоків на які будемо розділяти обробку даних.
    // Якщо рядків менше, тоді обробляємо в одному потоці
    int numTasks = 5;
    if (validationRows.Count >= numTasks)
    {
        int rowInTask = validationRows.Count / numTasks;

        Task<StringValidation.Validating>[] tasks = new Task<StringValidation.Validating>[numTasks];

        // Розділення даних на потоки
        for (int currentNumTask = 1; numTasks >= currentNumTask; currentNumTask++)
        {
            int startIndex = (currentNumTask - 1) * rowInTask;

            if (currentNumTask == numTasks)
            {
                // В останній поток додаємо залишок рядків
                rowInTask += validationRows.Count - rowInTask * currentNumTask;
            }

            List<StringValidation.Models.Row> partRowsToValidate = validationRows.GetRange(startIndex, rowInTask);
            
            Task<StringValidation.Validating> processingTask = GetProcessingTaskAsync(partRowsToValidate);

            tasks[currentNumTask - 1] = processingTask;
        }

        StringValidation.Validating[] validating = await Task.WhenAll(tasks);

        foreach (StringValidation.Validating validatingResult in validating)
        {
            numberOfValidRows += validatingResult.NumberOfValidRows;
            numberOfNotValidRows += validatingResult.NumberOfNotValidRows;
        }
    }
    else
    {
        StringValidation.Validating result = await GetProcessingTaskAsync(validationRows);

        numberOfValidRows += result.NumberOfValidRows;
        numberOfNotValidRows += result.NumberOfNotValidRows;
    }

    Console.WriteLine("Аналіз завершено\n");

    WriteProcessingResult(numberOfValidRows, numberOfNotValidRows);
}

/// <summary>
/// Створення задачі для перевірки даних 
/// </summary>
/// <param name="rowsToValidate">Список даних які потрібно перевірити</param>
static Task<StringValidation.Validating> GetProcessingTaskAsync(List<StringValidation.Models.Row> rowsToValidate)
{
    return Task.Run(() =>
    {
        StringValidation.Validating validating = new();
        validating.ValidateData(rowsToValidate.ToArray());

        return validating;
    });
}

/// <summary>
/// Вивід результату перевірки даних
/// </summary>
/// <param name="numIsValide">Кількість рядків які пройшли успішну перевірку</param>
/// <param name="numIsNotValidate">Кількість рядків які перевірку не пройшли</param>
static void WriteProcessingResult(int numberOfValidRows, int numberOfNotValidRows)
{
    Console.ForegroundColor = ConsoleColor.DarkGray;

    Console.WriteLine("Результат:\n" +
                     $"   -> Валідних рядків - {numberOfValidRows}\n" +
                     $"   -> Рядки які не пройшли перевірку - {numberOfNotValidRows}\n");
}
