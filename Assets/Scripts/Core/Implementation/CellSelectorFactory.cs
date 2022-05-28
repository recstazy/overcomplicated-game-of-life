using Zenject;
using GameOfLife.Abstraction;
using GameOfLife.Core.Ecs;
using System;

namespace GameOfLife.Core
{
    public class CellSelectorFactory : IFactory<GameImplementationType, ICellSelector>
    {
        private readonly DiContainer container;

        public CellSelectorFactory(DiContainer container)
        {
            this.container = container;
        }

        public ICellSelector Create(GameImplementationType type) => type switch
        {
            GameImplementationType.Ecs => container.Instantiate<EcsCellSelector>(),
            _ => throw new ArgumentException($"Couldn't find CellSelector for {type} implementation"),
        };
    }
}
