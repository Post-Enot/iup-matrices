namespace IUP.Toolkits.Matrices
{
    /// <summary>
    /// Перечисление, представляющее тип поворота матрицы.
    /// </summary>
    public enum MatrixRotation : byte
    {
        /// <summary>
        /// Поворот по часовой стрелке на 90 градусов.
        /// </summary>
        Clockwise90_Degrees = 0,
        /// <summary>
        /// Поворот против часовой стрелки на 90 градусов.
        /// </summary>
        Conterclockwise90_Degrees = 1,
        /// <summary>
        /// Поворот на 180 градусов.
        /// </summary>
        _180_Degrees = 2,
        /// <summary>
        /// Повторот по часовой стрелке на 270 градусов.
        /// </summary>
        Clockwise270_Degrees = 3,
        /// <summary>
        /// Поворот против часовой стрелки на 270 градусов.
        /// </summary>
        Conterclockwise270_Degrees = 4
    }
}
