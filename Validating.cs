namespace StringValidation
{
    /// <summary>
    /// Перевірка даних згідно шаблону
    /// </summary>
    internal class Validating
    {
        /// <summary>
        /// Масив валідних рядків
        /// </summary>
        private Models.Row[]? _validRows;     
        /// <summary>
        /// Масив не валідних рядків
        /// </summary>
        private Models.Row[]? _notValidRows;

        /// <summary>
        /// Кількість валідних рядків
        /// </summary>
        internal int NumberOfValidRows { get => _validRows?.Count(el => el != default) ?? 0; }      
        /// <summary>
        /// Кількість не валідних рядків
        /// </summary>
        internal int NumberOfNotValidRows { get => _notValidRows?.Count(el => el != default) ?? 0; }

        /// <summary>
        /// Валідація масиву даних
        /// </summary>
        /// <param name="data">Масив даних</param>
        internal void ValidateData(Models.Row[] data)
        {
            _validRows = new Models.Row[data.Length];
            _notValidRows = new Models.Row[data.Length];

            foreach (Models.Row itemData in data)
            {
                bool rowIsValid = ValidateRow(itemData);

                if (rowIsValid)
                    _validRows[NumberOfValidRows] = itemData;
                else
                    _notValidRows[NumberOfNotValidRows] = itemData;
            }
        }

        /// <summary>
        /// Валідація рядку даних
        /// </summary>
        /// <param name="itemData">Рядок даних</param>
        /// <returns>true - якщо рядко пройшов валідацію. false - якщо ні</returns>
        private static bool ValidateRow(Models.Row itemData)
        {
            bool rowIsValid = true;
            foreach (char symbol in itemData.ValidationSymbols)
            {
                int numSymbols = itemData.CheckString.Count(el => el.Equals(symbol));

                if (rowIsValid)
                {
                    rowIsValid = itemData.QuantityFrom <= numSymbols && numSymbols <= itemData.QuantityTo;
                }
            }

            return rowIsValid;
        }
    }
}
