using System;
using UnityEngine;

namespace GameOfLife
{
    [Serializable]
    public struct RangedValue<T> where T : struct
    {
        [SerializeField]
        private T defaultValue;

        [SerializeField]
        private T minValue;

        [SerializeField]
        private T maxValue;

        public T DefaultValue { get => defaultValue; }
        public T MinValue { get => minValue; }
        public T MaxValue { get => maxValue; }
    }
}
