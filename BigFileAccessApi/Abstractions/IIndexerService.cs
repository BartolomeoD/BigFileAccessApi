using BigFileAccessApi.Contracts;

namespace BigFileAccessApi.Abstractions
{
    public interface IIndexerService
    {
        /// <summary>
        /// Первичная индесация файла
        /// </summary>
        /// <param name="path"></param>
        void IndexFile(string path);

        /// <summary>
        /// Добавить иидекс строки в конец
        /// </summary>
        /// <param name="lineLength"></param>
        void AppendLine(int lineLength);

        /// <summary>
        /// Всавить строку по определенному индексу
        /// </summary>
        /// <param name="lineNumber"></param>
        /// <param name="lineLength"></param>
        void AddLine(int lineNumber, int lineLength);

        /// <summary>
        /// Получить данные о строке по индексу
        /// </summary>
        /// <param name="lineNumber"></param>
        /// <returns></returns>
        Line GetLine(int lineNumber);

        /// <summary>
        /// удалить строку из индекса
        /// </summary>
        /// <param name="lineNumber"></param>
        /// <param name="lineLength"></param>
        void DeleteLine(int lineNumber, int lineLength);
    }
}
