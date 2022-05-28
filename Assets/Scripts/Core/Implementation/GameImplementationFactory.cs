using GameOfLife.Abstraction;
using GameOfLife.Core.Ecs;
using System;

namespace GameOfLife.Core
{
    public class GameImplementationFactory : IGameImplementationFactory
    {
        public IGameImplementation Create(GameImplementationType type) => type switch
        {
            GameImplementationType.Ecs => new EcsGameImplementation(),
            _ => throw new ArgumentException($"Implementation for {type} type not found")
        };
    }
}
