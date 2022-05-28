using System;
using Unity.Collections;
using Unity.Mathematics;

namespace GameOfLife.Core
{
    public struct NativeGrid<T> : IDisposable where T : struct
    {
        public bool IsCreated => array.IsCreated;

        private NativeArray<T> array;
        private readonly int gridSize;

        public NativeGrid(int gridSize, Allocator allocator)
        {
            this.gridSize = gridSize;
            array = new NativeArray<T>(gridSize * gridSize, allocator);
        }

        public void Dispose()
        {
            if (!array.IsCreated)
                throw new ObjectDisposedException("Native Grid wasn't been initialized before dispose");

            array.Dispose();
        }

        public T this[int x, int y]
        {
            get => GetElement(x, y);
            set => SetElement(x, y, value);
        }

        public T this[int2 position]
        {
            get => GetElement(position.x, position.y);
            set => SetElement(position.x, position.y, value);
        }

        public void CostructFromArray(T[] array)
        {
            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    var arrayIndex = x + y;
                    if (arrayIndex >= array.Length)
                        return;

                    SetElement(x, y, in array[x + y]);
                }
            }
        }

        private T GetElement(int x, int y)
        {
            if (x < 0 || y < 0 || x >= gridSize || y >= gridSize)
                throw new IndexOutOfRangeException($"Grid coordinates [{x}, {y}] were out of range");

            return array[x * gridSize + y];
        }

        private void SetElement(int x, int y, in T value)
        {
            if (x < 0 || y < 0 || x >= gridSize || y >= gridSize)
                throw new IndexOutOfRangeException($"Grid coordinates [{x}, {y}] were out of range");

            array[x * gridSize + y] = value;
        }
    }
}
