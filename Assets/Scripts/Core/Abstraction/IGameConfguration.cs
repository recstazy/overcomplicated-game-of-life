using UnityEngine;

namespace GameOfLife.Abstraction
{
    public interface IGameConfguration
    {
        GameImplementationType Implementation { get; }
        int GridSize { get; }
        Mesh CellMesh { get; }
        Material CellMaterial { get; }
        Color AliveColor { get; }
        Color DeadColor { get; }
    }
}
