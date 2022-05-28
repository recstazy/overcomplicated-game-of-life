using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameOfLife.Abstraction
{
    public interface IGameOfLifeInput : IDisposable
    {
        event Action<Vector2Int> OnPointerDrag;
        event Action<bool> OnPointerHoldChanged;
        event Action OnPlayOrPause;
        event Action OnReset;
        Vector2Int CurrentPointerPosition { get; }
        void Update();
        void SetMouseInputActive(bool isActive);
    }
}
