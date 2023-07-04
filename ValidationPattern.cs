namespace StringValidation
{
    /// <summary>
    /// Перевірка даних на відповідність шаблону валідації
    /// </summary>
    internal class ValidationPattern
    {
        /// <summary>
        /// Попередня валідація даних. Перевірка рядків на корректність згідно шаблону
        /// </summary>
        /// <param name="data">Масив даних для перевірки</param>
        /// <returns>Список рядків які відповідають шаблону валідації</returns>
        internal List<Models.Row> ValidateData(string?[]? data)
        {
            List<Models.Row> rows = new();

            if (data != default)
            {
                foreach (string? itemRow in data)
                {
                    if (!string.IsNullOrEmpty(itemRow))
                    {
                        Models.Row? newData = ValidateRow(itemRow);

                        if (newData != null)
                        {
                            rows.Add(newData);
                        }
                    }
                }
            }
            return rows;
        }

        /// <summary>
        /// Можливі роздільники діапазону кількості
        /// </summary>
        private readonly char[] _quantitySeparators = new char[3] { '-', '/', '\\' };

        /// <summary>
        /// Перевірка прочитаного рядку файлу на корректність згідно шаблону
        /// </summary>
        /// <param name="row">Рядок даних</param>
        /// <returns>Рядок який відповідає шаблону</returns>
        private Models.Row? ValidateRow(string row)
        {
            string? symbolsToValidate = default;
            int quantityFrom = default;
            int quantityTo = default;
            string? checkString = default;

            string[] rowItems = row.Split(' ');

            if (rowItems.Length == 3)
            {
                try
                {
                    symbolsToValidate = rowItems[0];
                    string quantities = rowItems[1];
                    checkString = rowItems[2];

                    if (quantities.Length == 2)
                    {
                        quantityFrom = Convert.ToInt32(quantities.Replace(':', '\0'));
                        quantityTo = quantityFrom;
                    }
                    else if (quantities.Contains(':'))
                    {
                        string[] rowQuantities = quantities.Replace(':', '\0').Split(_quantitySeparators);
                        if (rowQuantities.Length == 2)
                        {
                            if (int.TryParse(rowQuantities[0], out int outQuantityFrom)
                                && int.TryParse(rowQuantities[1], out int outQuantityTo))
                            {
                                quantityFrom = outQuantityFrom;
                                quantityTo = outQuantityTo;

                                if (quantityFrom < 0
                                    || quantityFrom > quantityTo)
                                {
                                    quantityFrom = 0;
                                    quantityTo = 0;

                                    WriteError("Помилка введеного діапазону");
                                }
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    WriteError($"Помилка при аналізі рядку: {ex.Message}");
                }
            }

            if (!string.IsNullOrWhiteSpace(symbolsToValidate)
                && quantityFrom > 0
                && quantityTo > 0
                && !string.IsNullOrWhiteSpace(checkString))
            {
                return new(symbolsToValidate.ToArray(),
                           quantityFrom,
                           quantityTo,
                           checkString);
            }
            else
            {
                WriteError($"Рядок не відповідає шаблону: {row}");
            }

            return default;
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
