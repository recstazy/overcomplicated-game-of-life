using UnityEngine;
using System.Threading.Tasks;
using GameOfLife.Abstraction;

namespace GameOfLife.Core
{
    public class GameEntryPoint : MonoBehaviour
    {
        [SerializeField]
        private GameImplementationType implementationType;

        [SerializeField]
        [Min(0)]
        private int updateLoopDelay;

        [SerializeField]
        private bool isPaused;

        [SerializeField]
        private GameConfiguration config;

        [SerializeField]
        private Vector2Int[] aliveOnStartIndices;

        private IGameImplementation implementation;
        
        private void Awake()
        {
            var implementationFactory = new GameImplementationFactory();
            implementation = implementationFactory.CreateImplementation(implementationType);
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
