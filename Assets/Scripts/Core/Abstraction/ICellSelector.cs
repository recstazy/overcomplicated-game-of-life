using System;
using UnityEngine;

namespace GameOfLife.Abstraction
{
    public interface ICellSelector : IDisposable
    {
        event Action<Vector2Int[]> OnCellsWereSelected;
    }
}
