using System;
using UnityEngine;

namespace GameOfLife.Abstraction
{
    public interface IGameImplementation : IDisposable
    {
        IGameConfguration Configuration { get; set; }
        void Initialize();
        void ScheduleUpdate();
        void Reset(Vector2Int[] newAlivePositions);
    }
}
