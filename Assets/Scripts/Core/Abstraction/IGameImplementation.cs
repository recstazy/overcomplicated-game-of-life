using System;
using UnityEngine;

namespace GameOfLife.Abstraction
{
    public interface IGameImplementation : IDisposable
    {
        IGameConfiguration Configuration { get; set; }
        void Initialize();
        void ScheduleUpdate();
        void Reset(Vector2Int[] newAlivePositions);
        void FitToRect(Vector3 position, Quaternion rotation, Vector2 rectSize);
        void SetPixelResponseTime(float newResponseTime);
    }
}
