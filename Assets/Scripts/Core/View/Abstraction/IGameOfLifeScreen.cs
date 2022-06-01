using System;

namespace GameOfLife.Abstraction.View
{
    public interface IGameOfLifeScreen
    {
        event Action<int> OnUpdateIntervalChanged;
        void SetIsPlayting(bool isPlaying);
    }
}
