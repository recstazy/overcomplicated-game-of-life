using GameOfLife.Abstraction;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameOfLife.Input
{
    public class GameOfLifeInput : IGameOfLifeInput
    {
        public event Action<Vector2Int> OnPointerDrag;
        public event Action<bool> OnPointerHoldChanged;
        public event Action OnPlayOrPause;
        public event Action OnReset;

        public Vector2Int CurrentPointerPosition { get; private set; }
        private GameOfLifeControls controls;

        public GameOfLifeInput()
        {
            controls = new GameOfLifeControls();
            controls.GameOfLifeGame.HoldPointer.started += HoldPointerTriggered;
            controls.GameOfLifeGame.HoldPointer.canceled += HoldPointerTriggered;
            controls.GameOfLifeGame.PlayPauseSimulation.performed += PlayPausePerformed;
            controls.GameOfLifeGame.ResetSimulation.performed += ResetPerformed;
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

        public void SetMouseInputActive(bool isActive)
        {
            if (isActive)
                controls.GameOfLifeGame.HoldPointer.Enable();
            else
                controls.GameOfLifeGame.HoldPointer.Disable();
        }

        private void ResetPerformed(InputAction.CallbackContext context)
        {
            if (context.performed)
                OnReset?.Invoke();
        }

        private void PlayPausePerformed(InputAction.CallbackContext context)
        {
            if (context.performed)
                OnPlayOrPause?.Invoke();
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
