using Unity.Burst;
using Unity.Mathematics;

namespace GameOfLife.Core.Ecs
{
    [BurstCompile]
    public struct MathRect
    {
        public float2 Position;
        public float2 Size;

        public MathRect(float2 position, float2 size)
        {
            Position = position;
            Size = size;
        }

        public bool Contains(float2 point)
        {
            float2 bottomLeft = Position;
            float2 topRight = Position + Size;

            return point.x >= bottomLeft.x && point.y >= bottomLeft.y
                && point.x <= topRight.x && point.y <= topRight.y;
        }
    }
}
