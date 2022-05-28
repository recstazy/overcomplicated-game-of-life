using System;
using UnityEngine;

namespace GameOfLife.Abstraction
{
    public interface IGameImplementation : IDisposable
    {
        IGameConfguration Configuration { get; set; }
        void Initialize(Vector2Int[] aliveOnStartPositions);
        void ScheduleUpdate();
    }
}
