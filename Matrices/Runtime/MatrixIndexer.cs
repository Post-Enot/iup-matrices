using System;
using UnityEngine;

namespace IUP_Toolkits.Matrices
{
    /// <summary>
    /// Матричный индексатор для создания полей-индексаторов.
    /// </summary>
    /// <typeparam name="T">Тип элемента матричного индексатора.</typeparam>
    public class MatrixIndexer<T> : IReadonlyMatrixIndexer<T>
    {
        /// <summary>
        /// Создаёт матричный индексатор с getter- и setter-свойствами.
        /// </summary>
        /// <param name="getter">Делегат, принимающий координаты элемента матрицы в порядке 
        /// (x, y) и возвращает значение T.</param>
        /// <param name="setter">Делегат, принимающий координаты элемента матрицы в порядке 
        /// (x, y) и присваиваемый элемент T.</param>
        public MatrixIndexer(Func<int, int, T> getter, Action<int, int, T> setter)
        {
            _getter = getter;
            _setter = setter;
        }

        private readonly Func<int, int, T> _getter;
        private readonly Action<int, int, T> _setter;

        /// <summary>
        /// Матричный индексатор.
        /// </summary>
        /// <param name="coordinate">Координаты элемента матрицы.</param>
        public T this[Vector2Int coordinate]
        {
            get => _getter(coordinate.x, coordinate.y);
            set => _setter(coordinate.x, coordinate.y, value);
        }

        /// <summary>
        /// Матричный индексатор.
        /// </summary>
        /// <param name="x">Координата элемента матрицы по оси x.</param>
        /// <param name="y">Координата элемента матрицы по оси y.</param>
        public T this[int x, int y]
        {
            get => _getter(x, y);
            set => _setter(x, y, value);
        }
    }
}
