using Unity.Entities;
using Unity.Transforms;

namespace GameOfLife.Core.Ecs
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public class IsAliveSystem : SystemBase
    {
        private bool isUpdatePending;
        private int gridSize;

        public void SetGridSize(int gridSize)
        {
            this.gridSize = gridSize;
        }

        public void ScheduleUpdate()
        {
            isUpdatePending = true;
        }

        protected override void OnUpdate()
        {
            if (!isUpdatePending)
                return;

            isUpdatePending = false;

            var processor = new AliveProcessor(gridSize);

            // Fetch current isAlive states to the buffer
            Entities.ForEach((ref Translation t, in Cell cell) =>
            {
                processor.FetchIsAliveNow(in cell);
            }).Schedule();

            // Calculate all cells new alive states
            Entities.ForEach((in Cell cell) =>
            {
                processor.CalculateAliveNextIteratioin(cell.Position);
            }).Schedule();

            // Blit information from the new alive buffer to actual cells
            Entities.ForEach((ref Cell cell) =>
            {
                processor.BlitIsAlive(ref cell);
            }).WithDisposeOnCompletion(processor)
            .Schedule();
        }
    }
}
