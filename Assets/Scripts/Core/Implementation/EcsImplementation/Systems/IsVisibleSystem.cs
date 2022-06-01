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
        private float deathAnimationTime;

        public void SetColors(float4 aliveColor, float4 deadColor)
        {
            this.aliveColor = aliveColor;
            this.deadColor = deadColor;
        }

        public void SetDeathAnimationTime(float time)
        {
            deathAnimationTime = time;
        }

        protected override void OnUpdate()
        {
            var aliveColor = this.aliveColor;
            var deadColor = this.deadColor;
            var lerpFactorDecrement = Time.DeltaTime / deathAnimationTime;

            Entities.ForEach((ref UnlitColor materialColor, ref CellColorLerpFactor lerpFactor, in Cell cell) =>
            {
                if (cell.IsAlive)
                    lerpFactor.Value = 1f;
                else
                    lerpFactor.Value = math.clamp(lerpFactor.Value - lerpFactorDecrement, 0f, 1f);

                materialColor.Value = math.lerp(deadColor, aliveColor, lerpFactor.Value);
            }).Schedule();
        }
    }
}
