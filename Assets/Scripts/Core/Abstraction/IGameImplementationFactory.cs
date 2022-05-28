using Zenject;

namespace GameOfLife.Abstraction
{
    public interface IGameImplementationFactory : IFactory<GameImplementationType, IGameImplementation> { }

    public enum GameImplementationType
    {
        Ecs = 0,
    }
}
