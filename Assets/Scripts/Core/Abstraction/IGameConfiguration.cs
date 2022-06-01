using UnityEngine;

namespace GameOfLife.Abstraction
{
    public interface IGameConfiguration
    {
        GameImplementationType Implementation { get; }
        int GridSize { get; }
        float CellSize { get; }
        Mesh CellMesh { get; }
        Material CellMaterial { get; }
        Color AliveColor { get; }
        Color DeadColor { get; }
        int MinUpdateInterval { get; }
        int MaxUpdateInterval { get; }
        int DefaultUpdateInterval { get; }
        float DefaultPixelResponseTime { get; }
        float MinPixelResponseTime { get; }
        float MaxPixelResponseTime { get; }
    }
}
