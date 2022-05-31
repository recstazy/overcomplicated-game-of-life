using GameOfLife.Abstraction;
using GameOfLife.Abstraction.View;
using GameOfLife.Core;
using GameOfLife.Input;
using GameOfLife.View;
using UnityEngine;
using Zenject;

namespace GameOfLife
{
    public class GameOfLifeSceneInstaller : MonoInstaller
    {
        [SerializeField]
        private ScriptableGameConfiguration gameConfig;

        [SerializeField]
        private GameOfLiveScreen gameOfLifeView;

        public override void InstallBindings()
        {
            Container.Bind<IGameOfLifeScreen>().FromInstance(gameOfLifeView).AsSingle();

            Container.BindInterfacesTo<GameImplementationFactory>().AsTransient();
            Container.BindInterfacesTo<CellSelectorFactory>().AsTransient();
            Container.BindInterfacesTo<GameOfLifeInput>().AsSingle();
            Container.Bind<IGameConfiguration>().FromInstance(gameConfig).AsSingle();
        }
    }
}
