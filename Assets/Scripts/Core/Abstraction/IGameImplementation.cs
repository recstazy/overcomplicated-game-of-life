using UnityEngine;

namespace GameOfLife.Abstraction
{
    public interface IGameImplementation
    {
        IGameConfguration Configuration { get; set; }
        void Initialize(Vector2Int[] aliveOnStartPositions);
        void ScheduleUpdate();
    }
}
