using UnityEngine;
using System.Threading.Tasks;
using GameOfLife.Abstraction;
using Zenject;
using GameOfLife.Abstraction.View;

namespace GameOfLife.Core
{
    public class GameController : MonoBehaviour
    {
        [SerializeField]
        private Transform gameRectMin;

        [SerializeField]
        private Transform gameRectMax;

        private int updateInterval;
        
        private IGameConfiguration config;
        private IGameImplementationFactory implementationFactory;
        private IGameOfLifeInput input;
        private IGameImplementation implementation;
        private IGameOfLifeScreen view;
        private bool isPaused = true;

        [Inject]
        public void Construct(IGameConfiguration config, IGameImplementationFactory implementationFactory,
            IGameOfLifeInput input, IFactory<GameImplementationType, ICellSelector> cellSelectorFactory,
            IGameOfLifeScreen view)
        {
            this.config = config;
            this.implementationFactory = implementationFactory;
            this.input = input;
            this.view = view;
            cellSelectorFactory.Create(config.Implementation);

            input.OnReset += ResetGame;
            input.OnPlayOrPause += PlayPause;

            view.OnUpdateIntervalChanged += ChangeUpdateInterval;
            view.OnResponseTimeChanged += ChangeResponseTime;
        }

        private void Start()
        {
            SetPaused(true);
            implementation = implementationFactory.Create(config.Implementation);
            implementation.Configuration = config;
            implementation.Initialize();

            CalculateGameRect(out var position, out var rotation, out var size);
            implementation.FitToRect(position, rotation, size);

            ChangeUpdateInterval(config.DefaultUpdateInterval);
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

            if (view != null)
            {
                view.OnUpdateIntervalChanged -= ChangeUpdateInterval;
                view.OnResponseTimeChanged -= ChangeResponseTime;
            }
        }

        private void PlayPause()
        {
            SetPaused(!isPaused);
        }

        private void ResetGame()
        {
            SetPaused(true);
            implementation?.Reset(new Vector2Int[0]);
        }

        private void SetPaused(bool isPaused)
        {
            this.isPaused = isPaused;
            input.SetMouseInputActive(isPaused);
            view?.SetIsPlayting(!isPaused);
        }

        private async void StartUpdateLoop()
        {
            while (Application.isPlaying)
            {
                if (!isPaused)
                    implementation.ScheduleUpdate();

                await Task.Delay(updateInterval);
            }
        }

        private void CalculateGameRect(out Vector3 position, out Quaternion rotation, out Vector2 rectSize)
        {
            var rectSize3d = gameRectMax.position - gameRectMin.position;
            var forward = Vector3.Cross(rectSize3d, Vector3.up);

            position = gameRectMin.position;
            rotation = Quaternion.LookRotation(forward, Vector3.up);
            var matrix = Matrix4x4.TRS(position, rotation, Vector3.one);
            var localSize = matrix.MultiplyPoint3x4(gameRectMax.position) - matrix.MultiplyPoint3x4(gameRectMin.position);
            rectSize = localSize;
        }

        private void ChangeUpdateInterval(int newValue)
        {
            updateInterval = newValue;
        }

        private void ChangeResponseTime(float newResponseTime)
        {
            implementation.SetPixelResponseTime(newResponseTime);
        }
    }
}
