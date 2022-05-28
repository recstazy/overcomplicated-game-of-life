using UnityEngine;
using System.Threading.Tasks;
using GameOfLife.Abstraction;
using Zenject;
using System;

namespace GameOfLife.Core
{
    public class GameEntryPoint : MonoBehaviour
    {
        [SerializeField]
        [Min(0)]
        private int updateLoopDelay;

        [SerializeField]
        private Vector2Int[] aliveOnStartPositions;

        private IGameConfguration config;
        private IGameImplementationFactory implementationFactory;
        private IGameOfLifeInput input;
        private IGameImplementation implementation;
        private bool isPaused = true;

        [Inject]
        public void Construct(IGameConfguration config, IGameImplementationFactory implementationFactory,
            IGameOfLifeInput input)
        {
            this.config = config;
            this.implementationFactory = implementationFactory;
            this.input = input;

            input.OnReset += ResetGame;
            input.OnPlayOrPause += PlayPause;
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
                input.OnPlayOrPause -= PlayPause;
            }
        }

        private void PlayPause()
        {
            SetPaused(!isPaused);
        }

        private void ResetGame()
        {
            SetPaused(true);
            implementation?.Reset(aliveOnStartPositions);
        }

        private void SetPaused(bool isPaused)
        {
            this.isPaused = isPaused;
            input.SetMouseInputActive(isPaused);
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
