using GameOfLife.Abstraction;
using GameOfLife.Core.Ecs;
using System;
using Zenject;

namespace GameOfLife.Core
{
    public class GameImplementationFactory : IGameImplementationFactory
    {
        private readonly DiContainer container;

        public GameImplementationFactory(DiContainer container)
        {
            this.container = container;
        }

        public IGameImplementation Create(GameImplementationType type) => type switch
        {
            GameImplementationType.Ecs => container.Instantiate<EcsGameImplementation>(),
            _ => throw new ArgumentException($"Implementation for {type} type not found")
        };
    }
}
