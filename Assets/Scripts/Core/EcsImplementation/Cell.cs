using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace GameOfLife.Core.Ecs
{
    [Serializable]
    public struct Cell : IComponentData
    {
        public readonly int2 Position;
        public bool IsAlive;

        public Cell(int2 position)
        {
            Position = position;
            IsAlive = false;
        }
    }
}
