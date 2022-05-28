using GameOfLife.Abstraction;
using GameOfLife.Core.Ecs;
using System;

namespace GameOfLife
{
    public class GameImplementationFactory : IGameImplementationFactory
    {
        public IGameImplementation CreateImplementation(GameImplementationType type) => type switch
        {
            GameImplementationType.Ecs => new EcsGameImplementation(),
            _ => throw new ArgumentException($"Implementation for {type} type not found")
        };
    }
}
