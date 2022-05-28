using GameOfLife.Abstraction;
using UnityEngine;
using Zenject;
using GameOfLife.Input;

namespace GameOfLife.Core.Injection
{
    [CreateAssetMenu(fileName = "MainInstaller", menuName = "Installers/MainInstaller")]
    public class MainInstaller : ScriptableObjectInstaller<MainInstaller>
    {
        [SerializeField]
        private GameConfiguration configuration;

        public override void InstallBindings()
        {
            Container.BindInterfacesTo<GameImplementationFactory>().AsTransient();
            Container.BindInterfacesTo<GameOfLifeInput>().AsSingle();
            Container.Bind<IGameConfguration>().FromInstance(configuration).AsSingle();
        }
    }
}