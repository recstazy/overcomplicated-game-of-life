namespace GameOfLife.Abstraction
{
    public interface IGameImplementationFactory
    {
        IGameImplementation CreateImplementation(GameImplementationType type);
    }

    public enum GameImplementationType
    {
        Ecs = 0,
    }
}
