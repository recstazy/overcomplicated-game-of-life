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
        private Vector2Int[] aliveOnStartPositions;

        private IGameConfguration config;
        private IGameImplementationFactory implementationFactory;
        private IGameOfLifeInput input;
        private IGameImplementation implementation;

        [Inject]
        public void Construct(IGameConfguration config, IGameImplementationFactory implementationFactory,
            IGameOfLifeInput input)
        {
            this.config = config;
            this.implementationFactory = implementationFactory;
            this.input = input;

            input.OnReset += ResetGame;
        }

        private void Start()
        {
            implementation = implementationFactory.Create(config.Implementation);
            implementation.Configuration = config;
            implementation.Initialize();
            implementation.Reset(aliveOnStartPositions);
            StartUpdateLoop();
        }

        private void Update()
        {
            input?.Update();
        }

        private void OnDestroy()
        {
            implementation?.Dispose();

            if (input != null)
            {
                input.OnReset -= ResetGame;
            }
        }

        private void ResetGame()
        {
            implementation?.Reset(aliveOnStartPositions);
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
