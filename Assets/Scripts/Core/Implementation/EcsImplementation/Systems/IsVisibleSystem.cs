using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Rendering;

namespace GameOfLife.Core.Ecs
{
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public class IsVisibleSystem : SystemBase
    {
        private float4 aliveColor;
        private float4 deadColor;

        public void SetColors(float4 aliveColor, float4 deadColor)
        {
            this.aliveColor = aliveColor;
            this.deadColor = deadColor;
        }

        protected override void OnUpdate()
        {
            var aliveColor = this.aliveColor;
            var deadColor = this.deadColor;

            Entities.ForEach((ref UnlitColor materialColor, in Cell cell) =>
            {
                materialColor.Value = cell.IsAlive ? aliveColor : deadColor;
            }).Schedule();
        }
    }
}
