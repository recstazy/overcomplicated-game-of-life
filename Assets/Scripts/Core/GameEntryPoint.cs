using UnityEngine;
using System.Threading.Tasks;
using GameOfLife.Abstraction;
using Zenject;

namespace GameOfLife.Core
{
    public class GameEntryPoint : MonoBehaviour
    {
        [SerializeField]
        [Min(0)]
        private int updateLoopDelay;

        [SerializeField]
        private bool isPaused;

        [SerializeField]
        private Vector2Int[] aliveOnStartIndices;

        private IGameConfguration config;
        private IGameImplementationFactory implementationFactory;
        private IGameImplementation implementation;

        [Inject]
        public void Construct(IGameConfguration config, IGameImplementationFactory implementationFactory)
        {
            this.config = config;
            this.implementationFactory = implementationFactory;
        }

        private void Start()
        {
            implementation = implementationFactory.Create(config.Implementation);
            implementation.Configuration = config;
            implementation.Initialize(aliveOnStartIndices);
            StartUpdateLoop();
        }

        private async void StartUpdateLoop()
        {
            while (Application.isPlaying)
            {
                if (!isPaused)
                    implementation.ScheduleUpdate();

                await Task.Delay(updateLoopDelay);
            }
        }
    }
}
