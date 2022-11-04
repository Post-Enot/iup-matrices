using System.Collections.Generic;

namespace IUP_Toolkits.Matrices
{
    /// <summary>
    /// Readonly-интерфейс матрицы.
    /// </summary>
    /// <typeparam name="T">Тип данных элементов матрицы.</typeparam>
    public interface IReadonlyMatrix<T> : IReadonlyMatrixIndexer<T>, IReadOnlyCollection<T> { }
}
