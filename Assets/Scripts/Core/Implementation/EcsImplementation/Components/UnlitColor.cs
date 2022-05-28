using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;

namespace GameOfLife.Core
{
    [MaterialProperty("_UnlitColor", MaterialPropertyFormat.Float4)]
    public struct UnlitColor : IComponentData
    {
        public float4 Value;
    }
}