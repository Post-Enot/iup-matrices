using UnityEngine;

namespace IUP_Toolkits.Matrices
{
    /// <summary>
    /// Readonly-интерфейс матричного индексатора.
    /// </summary>
    /// <typeparam name="T">Тип данных элементов матрицы.</typeparam>
    public interface IReadonlyMatrixIndexer<T>
    {
        /// <summary>
        /// Индексатор для readonly-доступа к элементам матрицы.
        /// </summary>
        /// <param name="coordinate">Координаты элемента матрицы.</param>
        public T this[Vector2Int coordinate] { get; }

        /// <summary>
        /// Индексатор для readonly-доступа к элементам матрицы.
        /// </summary>
        /// <param name="x">Координата элемента матрицы по оси x.</param>
        /// <param name="y">Координата элемента матрицы по оси y.</param>
        public T this[int x, int y] { get; }
    }
}
