using System;

namespace GameOfLife.Abstraction.View
{
    public interface IGameOfLifeScreen
    {
        event Action<int> OnUpdateIntervalChanged;
        event Action<float> OnResponseTimeChanged;
        void SetIsPlayting(bool isPlaying);
    }
}
