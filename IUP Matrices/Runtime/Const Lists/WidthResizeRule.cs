﻿namespace IUP.Toolkits.Matrices
{
    /// <summary>
    /// Перечисление, представляющее правило смещения по ширине при изменении размера матрицы.
    /// </summary>
    public enum WidthResizeRule : byte
  
    {
        /// <summary>
        /// Смещение происходит по левую сторону.
        /// </summary>
        Left = 0,
        /// <summary>
        /// Смещение происходит по правую сторону.
        /// </summary>
        Right = 1,
        /// <summary>
        /// Смещение происходит по центру; в случае смещения на нечётное число приоритет отдаётся левой стороне.
        /// </summary>
        CenterLeft = 2,
        /// <summary>
        /// Смещение происходит по центру; в случае смещения на нечётное число приоритет отдаётся правой стороне.
        /// </summary>
        CenterRight = 3
    }
}
