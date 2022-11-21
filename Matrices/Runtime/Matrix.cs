using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IUP.Toolkits.Matrices
{
    /// <summary>
    /// Оболочка для матриц.
    /// </summary>
    /// <typeparam name="T">Тип данных элементов матрицы.</typeparam>
    public class Matrix<T> : IReadonlyMatrix<T>
    {
        /// <summary>
        /// Создаёт пустую матрицу размерностью [0, 0].
        /// </summary>
        public Matrix()
        {
            _matrix = new T[0, 0];
        }

        /// <summary>
        /// Создаёт матрицу переданного размера.
        /// </summary>
        /// <param name="width">Ширина матрицы (количество элементов в строке; количество столбцов).</param>
        /// <param name="height">Высота матрицы (количество элементов в столбце; количество строк).</param>
        public Matrix(int width, int height)
        {
            _matrix = new T[height, width];
        }

        /// <summary>
        /// Ширина матрицы (количество элементов в строке; количество столбцов).
        /// </summary>
        public int Width => _matrix.GetLength(1);
        /// <summary>
        /// Высота матрицы (количество элементов в столбце; количество строк).
        /// </summary>
        public int Height => _matrix.GetLength(0);

        public int Count => _matrix.Length;

        private T[,] _matrix;

        /// <summary>
        /// Индексатор для доступа к элементам матрицы.
        /// </summary>
        /// <param name="coordinate">Координаты элемента матрицы.</param>
        /// <returns>Возвращает элемент матрицы по переданным координатам.</returns>
        public T this[Vector2Int coordinate]
        {
            get => _matrix[coordinate.y, coordinate.x];
            set => _matrix[coordinate.y, coordinate.x] = value;
        }

        /// <summary>
        /// Индексатор для доступа к элементам матрицы.
        /// </summary>
        /// <param name="x">Координата элемента матрицы по оси x.</param>
        /// <param name="y">Координата элемента матрицы по оси y.</param>
        /// <returns>Возвращает элемент матрицы по переданным координатам.</returns>
        public T this[int x, int y]
        {
            get => _matrix[y, x];
            set => _matrix[y, x] = value;
        }

        /// <summary>
        /// Пересоздаёт матрицу, сбрасывая значения.
        /// <param name="width">Ширина матрицы (количество элементов в строке).</param>
        /// <param name="height">Высота матрицы (количество элементов в столбце).</param>
        /// </summary>
        public void Recreate(int width, int height)
        {
            _matrix = new T[height, width];
        }

        /// <summary>
        /// Изменяет размер матрицы с учётом направления смещения.
        /// </summary>
        /// <param name="widthOffset"></param>
        /// <param name="heightOffset"></param>
        public void Resize(int widthOffset, int heightOffset) { }

        /// <summary>
        /// Возвращает ссылку на двухмерный массив матрицы.
        /// </summary>
        /// <returns>Ссылка на двухмерный массив матрицы.</returns>
        public T[,] GetDoubleArray()
        {
            return _matrix;
        }

        /// <summary>
        /// Преобразует двухмерный массив матрицы в одномерный массив и возвращает ссылку на него.
        /// </summary>
        /// <returns>Ссылка на одномерный массив элементов матрицы.</returns>
        public T[] ToArray()
        {
            var array = new T[Width * Height];
            for (int i = 0; i < array.Length; i += 1)
            {
                int x = i % Width;
                int y = i / Width;
                array[i] = _matrix[y, x];
            }
            return array;
        }

        /// <summary>
        /// Проверяет, находятся ли координата в области определения матрицы.
        /// </summary>
        /// <param name="coordinate">Проверяемая координата.</param>
        /// <returns>Возвращает true, если координата находится в области определния матрицы; иначе false.</returns>
        public bool IsCoordinateInDefinitionDomain(Vector2Int coordinate)
        {
            return coordinate.x >= 0 && coordinate.x < Width && coordinate.y >= 0 && coordinate.y < Height;
        }

        /// <summary>
        /// Проверяет, находятся ли координата в области определения матрицы.
        /// </summary>
        /// <param name="x">X-компонент проверяемой координаты.</param>
        /// <param name="y">Y-компонент проверяемой координаты.</param>
        /// <returns>Возвращает true, если координата находится в области определния матрицы; иначе false.</returns>
        public bool IsCoordinateInDefinitionDomain(int x, int y)
        {
            return x >= 0 && x < Width && y >= 0 && y < Height;
        }

        public IEnumerator<T> GetEnumerator()
        {
            throw new System.NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new System.NotImplementedException();
        }
    }
}
