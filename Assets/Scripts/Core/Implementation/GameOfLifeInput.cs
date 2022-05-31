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
        public bool IsAddingCells { get; private set; }
        private GameOfLifeControls controls;

        public GameOfLifeInput()
        {
            controls = new GameOfLifeControls();
            controls.GameOfLifeGame.HoldAddCells.started += HoldAddCellsTriggered;
            controls.GameOfLifeGame.HoldAddCells.canceled += HoldAddCellsTriggered;
            controls.GameOfLifeGame.HoldRemoveCells.started += HoldRemoveCellsTriggered;
            controls.GameOfLifeGame.HoldRemoveCells.canceled += HoldRemoveCellsTriggered;
            controls.GameOfLifeGame.PlayPauseSimulation.performed += PlayPausePerformed;
            controls.GameOfLifeGame.ResetSimulation.performed += ResetPerformed;
            controls.Enable();
        }

        public void Update()
        {
            if (controls.GameOfLifeGame.HoldAddCells.IsPressed() || controls.GameOfLifeGame.HoldRemoveCells.IsPressed())
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
                controls.GameOfLifeGame.HoldAddCells.started -= HoldAddCellsTriggered;
                controls.GameOfLifeGame.HoldAddCells.canceled -= HoldAddCellsTriggered;
                controls.GameOfLifeGame.HoldRemoveCells.started -= HoldRemoveCellsTriggered;
                controls.GameOfLifeGame.HoldRemoveCells.canceled -= HoldRemoveCellsTriggered;
                controls.Dispose();
            }
        }

        public void SetMouseInputActive(bool isActive)
        {
            if (isActive)
            {
                controls.GameOfLifeGame.HoldAddCells.Enable();
                controls.GameOfLifeGame.HoldRemoveCells.Enable();
            }
            else
            {
                controls.GameOfLifeGame.HoldAddCells.Disable();
                controls.GameOfLifeGame.HoldRemoveCells.Disable();
            }
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

        private void HoldAddCellsTriggered(InputAction.CallbackContext context)
        {
            if (controls.GameOfLifeGame.HoldRemoveCells.IsPressed())
                return;

            IsAddingCells = true;

            if (context.started)
                OnPointerHoldChanged?.Invoke(true);
            else if (context.canceled)
                OnPointerHoldChanged?.Invoke(false);
        }

        private void HoldRemoveCellsTriggered(InputAction.CallbackContext context)
        {
            if (controls.GameOfLifeGame.HoldAddCells.IsPressed())
                return;

            IsAddingCells = false;

            if (context.started)
                OnPointerHoldChanged?.Invoke(true);
            else if (context.canceled)
                OnPointerHoldChanged?.Invoke(false);
        }
    }
}
