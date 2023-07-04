namespace StringValidation.Models
{
    /// <summary>
    /// Клас шаблону перевірки даних
    /// </summary>
    internal class Row
    {
        /// <summary>
        /// Конструктор перевірки даних
        /// </summary>
        /// <param name="symbols">Масив символів для перевірки</param>
        /// <param name="quantityFrom">Кількість символів від</param>
        /// <param name="quantityTo">Кількість символів до</param>
        /// <param name="checkString">Рядок перевірки</param>
        internal Row(char[] symbols,
                   int quantityFrom,
                   int quantityTo,
                   string checkString)
        {
            ValidationSymbols = symbols;
            QuantityFrom = quantityFrom;
            QuantityTo = quantityTo;
            CheckString = checkString;
        }

        /// <summary>
        /// Масив символів для перевірки
        /// </summary>
        internal char[] ValidationSymbols { get; set; }
        /// <summary>
        /// Кількість символів від
        /// </summary>
        internal int QuantityFrom { get; set; }
        /// <summary>
        /// Кількість символів до
        /// </summary>
        internal int QuantityTo { get; set; }
        /// <summary>
        /// Рядок перевірки
        /// </summary>
        internal string CheckString { get; set; }
    }
}
