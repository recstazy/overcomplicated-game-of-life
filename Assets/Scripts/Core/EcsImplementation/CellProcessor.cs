using GameOfLife.Core.Ecs;
using Unity.Collections;
using Unity.Mathematics;
using System;
using GameOfLife.Core.Models;

namespace GameOfLife.Core
{
    public struct AliveProcessor : IDisposable
    {
        private NativeGrid<bool> isAliveNowBuffer;
        private NativeGrid<bool> isAliveNextIterationBuffer;
        private readonly int gridSize;

        public AliveProcessor(int gridSize)
        {
            this.gridSize = gridSize;
            isAliveNowBuffer = new NativeGrid<bool>(gridSize, Allocator.TempJob);
            isAliveNextIterationBuffer = new NativeGrid<bool>(gridSize, Allocator.TempJob);
        }

        public void Dispose()
        {
            isAliveNowBuffer.Dispose();
            isAliveNextIterationBuffer.Dispose();
        }

        public void FetchIsAliveNow(in Cell cell)
        {
            isAliveNowBuffer[cell.Position] = cell.IsAlive;
        }

        public void CalculateAliveNextIteratioin(int2 position)
        {
            ushort aliveNeighboursCount = CountAliveNeighbours(position);
            bool isAliveNow = isAliveNowBuffer[position];
            bool isAliveNextIteration;

            if (isAliveNow)
                isAliveNextIteration = aliveNeighboursCount >= 2 && aliveNeighboursCount <= 3;
            else
                isAliveNextIteration = aliveNeighboursCount == 3;

            isAliveNextIterationBuffer[position] = isAliveNextIteration;
        }

        public void BlitIsAlive(ref Cell cell)
        {
            cell.IsAlive = isAliveNextIterationBuffer[cell.Position];
        }

        private ushort CountAliveNeighbours(int2 position)
        {
            int startX = position.x - 1;
            int startY = position.y - 1;
            int endX = position.x + 1;
            int endY = position.y + 1;
            ushort aliveNeighbours = 0;

            for (int sourceX = startX; sourceX <= endX; sourceX++)
            {
                var x = LoopGridPosition(sourceX);

                for (int sourceY = startY; sourceY <= endY; sourceY++)
                {
                    var y = LoopGridPosition(sourceY);

                    if (x == position.x && y == position.y)
                        continue;

                    if (isAliveNowBuffer[x, y])
                        aliveNeighbours++;

                    // Early exit because we dont care about values bigger than 4
                    // According to rules of Game Of Life
                    if (aliveNeighbours == 4)
                        return aliveNeighbours;
                }
            }

            return aliveNeighbours;
        }

        private int LoopGridPosition(int sourcePosition)
        {
            if (sourcePosition < 0)
                return sourcePosition + gridSize;
            else if (sourcePosition >= gridSize)
                return sourcePosition - gridSize;
            else
                return sourcePosition;
        }
    }
}
