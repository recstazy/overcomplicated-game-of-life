using GameOfLife.Abstraction;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameOfLife.Input
{
    public class GameOfLifeInput : IGameOfLifeInput
    {
        public Vector2Int CurrentPointerPosition { get; private set; }
        public event Action<Vector2Int> OnPointerDrag;
        public event Action<bool> OnPointerHoldChanged;

        private GameOfLifeControls controls;

        public GameOfLifeInput()
        {
            controls = new GameOfLifeControls();
            controls.GameOfLifeGame.HoldPointer.started += HoldPointerTriggered;
            controls.GameOfLifeGame.HoldPointer.canceled += HoldPointerTriggered;
            controls.Enable();
        }

        public void Update()
        {
            if (controls.GameOfLifeGame.HoldPointer.IsPressed())
            {
                var newPointerPosition = Vector2Int.RoundToInt(controls.GameOfLifeGame.PointerPosition.ReadValue<Vector2>());

                if (newPointerPosition != CurrentPointerPosition)
                {
                    CurrentPointerPosition = newPointerPosition;
                    OnPointerDrag?.Invoke(CurrentPointerPosition);
                }
            }
        }

        public void Dispose()
        {
            if (controls != null)
            {
                controls.GameOfLifeGame.HoldPointer.started -= HoldPointerTriggered;
                controls.GameOfLifeGame.HoldPointer.canceled -= HoldPointerTriggered;
                controls.Dispose();
            }
        }

        private void HoldPointerTriggered(InputAction.CallbackContext context)
        {
            if (context.started)
                OnPointerHoldChanged?.Invoke(true);
            else if (context.canceled)
                OnPointerHoldChanged?.Invoke(false);
        }
    }
}
