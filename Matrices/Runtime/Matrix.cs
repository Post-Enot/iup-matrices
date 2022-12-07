using System;
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
        /// <param name="widthOffset">Изменение ширины: если значение положительное, в матрицу добавляются 
        /// столбцы в соответствии с правилом изменения; если отрицательное - столбцы удаляются.</param>
        /// <param name="heightOffset">Изменение высоты: если значение положительное, в матрицу добавляются 
        /// строки в соответствии с правилом изменения; если отрицательное - строки удаляются.</param>
        /// <param name="widthResizeRule">Правило изменения ширины.</param>
        /// <param name="heightResizeRule">Правило изменения высоты.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void Resize(
            int widthOffset = 0,
            int heightOffset = 0,
            WidthResizeRule widthResizeRule = WidthResizeRule.Left,
            HeightResizeRule heightResizeRule = HeightResizeRule.Up)
        {
            int updatedWidth = Width + widthOffset;
            if (updatedWidth < 0)
            {
                updatedWidth = 0;
            }
            int updatedHeight = Height + heightOffset;
            if (updatedHeight < 0)
            {
                updatedHeight = 0;
            }

            const int startOffsetCode = 0;
            const int endOffsetCode = 1;
            const int centerStartOffsetCode = 2;
            const int centerEndOffsetCode = 3;

            static void CalculateMatrixComponentParams(
                int oldSize,
                int updatedSize,
                int componentOffset,
                int resizeRuleCode,
                out int componentIterCount,
                out int oldMatrixComponentOffset,
                out int resizedMatrixComponentOffset)
            {
                componentIterCount = updatedSize;
                oldMatrixComponentOffset = 0;
                resizedMatrixComponentOffset = 0;
                if (updatedSize > 0 && updatedSize != oldSize)
                {
                    componentIterCount = componentOffset >= 0 ? oldSize : updatedSize;
                    switch (resizeRuleCode)
                    {
                        case startOffsetCode:
                            if (componentOffset > 0)
                            {
                                resizedMatrixComponentOffset = componentOffset;
                            }
                            else
                            {
                                oldMatrixComponentOffset = -componentOffset;
                            }
                            return;

                        case endOffsetCode:
                            return;

                        case centerStartOffsetCode:
                            if (componentOffset > 0)
                            {
                                resizedMatrixComponentOffset = componentOffset / 2;
                                if (componentOffset.IsOdd())
                                {
                                    resizedMatrixComponentOffset += 1;
                                }
                            }
                            else
                            {
                                oldMatrixComponentOffset = -componentOffset / 2;
                                if (componentOffset.IsOdd())
                                {
                                    oldMatrixComponentOffset += 1;
                                }
                            }
                            return;

                        case centerEndOffsetCode:
                            if (componentOffset > 0)
                            {
                                resizedMatrixComponentOffset = componentOffset / 2;
                            }
                            else
                            {
                                oldMatrixComponentOffset = -componentOffset / 2;
                            }
                            return;

                        default:
                            throw new ArgumentOutOfRangeException(nameof(resizeRuleCode));
                    }
                }
            }

            int widthResizeRuleCode = widthResizeRule switch
            {
                WidthResizeRule.Left => startOffsetCode,
                WidthResizeRule.Right => endOffsetCode,
                WidthResizeRule.CenterLeft => centerStartOffsetCode,
                WidthResizeRule.CenterRight => centerEndOffsetCode,
                _ => throw new ArgumentOutOfRangeException(nameof(widthResizeRule))
            };
            CalculateMatrixComponentParams(
                Width,
                updatedWidth,
                widthOffset,
                widthResizeRuleCode,
                out int xIterCount,
                out int oldMatrixX_Offset,
                out int resizedMatrixX_Offset);

            int heightResizeRuleCode = heightResizeRule switch
            {
                HeightResizeRule.Down => startOffsetCode,
                HeightResizeRule.Up => endOffsetCode,
                HeightResizeRule.CenterDown => centerStartOffsetCode,
                HeightResizeRule.CenterUp => centerEndOffsetCode,
                _ => throw new ArgumentOutOfRangeException(nameof(widthResizeRule))
            };
            CalculateMatrixComponentParams(
                Height,
                updatedHeight,
                heightOffset,
                heightResizeRuleCode,
                out int yIterCount,
                out int oldMatrixY_Offset,
                out int resizedMatrixY_Offset);

            T[,] resizedMatrix = new T[updatedHeight, updatedWidth];
            for (int y = 0; y < yIterCount; y += 1)
            {
                for (int x = 0; x < xIterCount; x += 1)
                {
                    resizedMatrix[y + resizedMatrixY_Offset, x + resizedMatrixX_Offset] =
                        _matrix[y + oldMatrixY_Offset, x + oldMatrixX_Offset];
                }
            }
            _matrix = resizedMatrix;
        }

        /// <summary>
        /// Поворачивает матрицу.
        /// </summary>
        /// <param name="matrixRotation">Тип вращения матрицы.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void Rotate(MatrixRotation matrixRotation)
        {
            int width;
            int height;
            if (matrixRotation is MatrixRotation.Clockwise90_Degrees
                or MatrixRotation.Clockwise270_Degrees
                or MatrixRotation.Conterclockwise90_Degrees
                or MatrixRotation.Conterclockwise270_Degrees)
            {
                width = Height;
                height = Width;
            }
            else
            {
                width = Width;
                height = Height;
            }
            var rotatedMatrix = new T[height, width];

            Func<int, int, int> calculateRotatedX = matrixRotation switch
            {
                MatrixRotation.Clockwise90_Degrees or
                    MatrixRotation.Conterclockwise270_Degrees => (int x, int y) => y,
                MatrixRotation.Conterclockwise90_Degrees or
                    MatrixRotation.Clockwise270_Degrees => (int x, int y) => Height - 1 - y,
                MatrixRotation._180_Degrees => (int x, int y) => Width - 1 - x,
                _ => throw new ArgumentOutOfRangeException(nameof(matrixRotation))
            };
            Func<int, int, int> calculateRotatedY = matrixRotation switch
            {
                MatrixRotation.Clockwise90_Degrees or
                    MatrixRotation.Conterclockwise270_Degrees => (int x, int y) => Width - 1 - x,
                MatrixRotation.Conterclockwise90_Degrees or
                    MatrixRotation.Clockwise270_Degrees => (int x, int y) => x,
                MatrixRotation._180_Degrees => (int x, int y) => Height - 1 - y,
                _ => throw new ArgumentOutOfRangeException(nameof(matrixRotation))
            };

            for (int y = 0; y < Height; y += 1)
            {
                for (int x = 0; x < Width; x += 1)
                {
                    int rotatedX = calculateRotatedX(x, y);
                    int rotatedY = calculateRotatedY(x, y);
                    rotatedMatrix[rotatedY, rotatedX] = _matrix[y, x];
                }
            }
            _matrix = rotatedMatrix;
        }

        /// <summary>
        /// Отражает матрицу.
        /// </summary>
        /// <param name="matrixMirror">Тип отражения матрицы.</param>
        public void Mirror(MatrixMirror matrixMirror)
        {
            if (matrixMirror is MatrixMirror.Horizontal)
            {
                MirrorHorizontally();
            }
            else if (matrixMirror is MatrixMirror.Vertical)
            {
                MirrorVertically();
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(matrixMirror));
            }
        }

        /// <summary>
        /// Инициализирует все значения матрицы с помощью переданной функции.
        /// </summary>
        /// <param name="initFuncion">Функция инициализации. Принимает два аргумента: x (int) и y (int), 
        /// представляющие индексы инициализируемого объекта в матрице.</param>
        public void InitAllElements(Func<int, int, T> initFuncion)
        {
            for (int y = 0; y < Height; y += 1)
            {
                for (int x = 0; x < Width; x += 1)
                {
                    _matrix[y, x] = initFuncion(x, y);
                }
            }
        }

        /// <summary>
        /// Инициализирует все значения матрицы с помощью переданной функции.
        /// </summary>
        /// <param name="initFuncion">Функция инициализации.</param>
        public void InitAllElements(Func<T> initFuncion)
        {
            for (int y = 0; y < Height; y += 1)
            {
                for (int x = 0; x < Width; x += 1)
                {
                    _matrix[y, x] = initFuncion();
                }
            }
        }

        /// <summary>
        /// Совершает операцию над каждым элементом матрицы.
        /// </summary>
        /// <param name="actionOnElement">Делегат операции, совершаемой над каждым элементом матрицы. 
        /// Принимает один аргумент: элемент матрицы (ref element).</param>
        public void ForEachElements(MapAction<T> actionOnElement)
        {
            for (int y = 0; y < Height; y += 1)
            {
                for (int x = 0; x < Width; x += 1)
                {
                    actionOnElement(ref _matrix[y, x]);
                }
            }
        }

        /// <summary>
        /// Совершает операцию над каждым элементом матрицы.
        /// </summary>
        /// <param name="actionOnElement">Делегат операции, совершаемой над каждым элементом матрицы. 
        /// Принимает три аргумента: сам элемент матрицы (ref element), а также две координаты элемента 
        /// (x, y).</param>
        public void ForEachElements(MapAction<T, int, int> actionOnElement)
        {
            for (int y = 0; y < Height; y += 1)
            {
                for (int x = 0; x < Width; x += 1)
                {
                    actionOnElement(ref _matrix[y, x], x, y);
                }
            }
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
            return (IEnumerator<T>)_matrix.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _matrix.GetEnumerator();
        }

        private void MirrorVertically()
        {
            int heightHalf = Height / 2;
            for (int y = 0; y < heightHalf; y += 1)
            {
                for (int x = 0; x < Width; x += 1)
                {
                    int swappedY = Height - 1 - y;
                    (_matrix[y, x], _matrix[swappedY, x]) = (_matrix[swappedY, x], _matrix[y, x]);
                }
            }
        }

        private void MirrorHorizontally()
        {
            int widthHalf = Width / 2;
            for (int y = 0; y < Height; y += 1)
            {
                for (int x = 0; x < widthHalf; x += 1)
                {
                    int swappedX = Width - 1 - x;
                    (_matrix[y, x], _matrix[y, swappedX]) = (_matrix[y, swappedX], _matrix[y, x]);
                }
            }
        }
    }
}
