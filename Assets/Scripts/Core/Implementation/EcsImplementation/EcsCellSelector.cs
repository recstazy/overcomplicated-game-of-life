using GameOfLife.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using UnityEngine;

namespace GameOfLife.Core.Ecs
{
    [DisableAutoCreation]
    public class EcsCellSelector : SystemBase, ICellSelector
    {
        public event Action<Vector2Int[]> OnCellsWereSelected;

        private readonly IGameOfLifeInput input;
        private bool isHoldingPointerDown;

        public EcsCellSelector(IGameOfLifeInput input)
        {
            this.input = input;
            World.DefaultGameObjectInjectionWorld.AddSystem(this);

            input.OnPointerHoldChanged += PointerHoldChanged;
            input.OnPointerDrag += PointerDrag;
        }

        private void PointerHoldChanged(bool isDown)
        {
            isHoldingPointerDown = isDown;
            Debug.Log(isDown);
        }

        private void PointerDrag(Vector2Int newPosition)
        {
            Debug.Log(newPosition);
        }

        public void Dispose()
        {
            if (input != null)
            {
                input.OnPointerHoldChanged -= PointerHoldChanged;
                input.OnPointerDrag -= PointerDrag;
            }
        }

        protected override void OnUpdate()
        {
            
        }
    }
}
