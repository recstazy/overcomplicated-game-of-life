using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace GameOfLife.Core.Ecs
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public class IsAliveSystem : SystemBase
    {
        private bool isUpdatePending;
        private bool isResetPending;
        private int gridSize;
        private int2[] resetAliveStatesBuffer;

        public void SetGridSize(int gridSize)
        {
            this.gridSize = gridSize;
        }

        public void ScheduleUpdate()
        {
            isUpdatePending = true;
        }

        public void Reset(in int2[] alivePositions)
        {
            isResetPending = true;
            resetAliveStatesBuffer = alivePositions;
            ScheduleUpdate();
        }

        protected override void OnUpdate()
        {
            if (!isUpdatePending)
                return;

            isUpdatePending = false;
            var processor = new AliveProcessor(gridSize, !isResetPending);
            
            if (isResetPending)
            {
                NativeGrid<bool> newIsAliveGrid = new NativeGrid<bool>(gridSize, Allocator.TempJob);

                foreach (var pos in resetAliveStatesBuffer)
                    newIsAliveGrid[pos.x, pos.y] = true;

                resetAliveStatesBuffer = null;
                processor.OverrideNextIterationAliveBuffer(newIsAliveGrid);
            }
            else
            {
                // Fetch current isAlive states to the buffer
                Entities.ForEach((in Cell cell) => processor.FetchIsAliveNow(in cell))
                    .Schedule();

                // Calculate all cells new alive states
                Entities.ForEach((in Cell cell) => processor.CalculateAliveNextIteratioin(cell.Position))
                    .Schedule();
            }

            isResetPending = false;

            // Blit information from the next iteration alive buffer to actual cells
            Entities.ForEach((ref Cell cell) => processor.BlitIsAlive(ref cell))
                .WithDisposeOnCompletion(processor)
                .Schedule();
        }
    }
}
