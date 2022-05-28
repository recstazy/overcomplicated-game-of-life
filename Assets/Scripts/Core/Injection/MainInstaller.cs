using GameOfLife.Abstraction;
using UnityEngine;
using Zenject;

namespace GameOfLife.Core.Injection
{
    [CreateAssetMenu(fileName = "MainInstaller", menuName = "Installers/MainInstaller")]
    public class MainInstaller : ScriptableObjectInstaller<MainInstaller>
    {
        [SerializeField]
        private GameConfiguration configuration;

        public override void InstallBindings()
        {
            Container.Bind<IGameImplementationFactory>().To<GameImplementationFactory>().AsTransient();
            Container.Bind<IGameConfguration>().FromInstance(configuration).AsSingle();
        }
    }
}