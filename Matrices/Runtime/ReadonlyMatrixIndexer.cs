using System;
using UnityEngine;

namespace IUP_Toolkits.Matrices
{
    /// <summary>
    /// Матричный индексатор для создания readonly-полей-индексаторов.
    /// </summary>
    /// <typeparam name="T">Тип элемента матричного индексатора.</typeparam>
    public class ReadonlyMatrixIndexer<T> : IReadonlyMatrixIndexer<T>
    {
        /// <summary>
        /// Создаёт матричный индексатор с getter- и setter-свойствами.
        /// </summary>
        /// <param name="getter">Делегат, принимающий координаты элемента матрицы в порядке 
        /// (x, y) и возвращает значение T.</param>
        public ReadonlyMatrixIndexer(Func<int, int, T> getter)
        {
            _getter = getter;
        }

        private readonly Func<int, int, T> _getter;

        /// <summary>
        /// Матричный индексатор для readonly-доступа.
        /// </summary>
        /// <param name="coordinate">Координаты элемента матрицы.</param>
        public T this[Vector2Int coordinate] => _getter(coordinate.x, coordinate.y);

        /// <summary>
        /// Матричный индексатор для readonly-доступа.
        /// </summary>
        /// <param name="x">Координата элемента матрицы по оси x.</param>
        /// <param name="y">Координата элемента матрицы по оси y.</param>
        public T this[int x, int y] => _getter(x, y);
    }
}
