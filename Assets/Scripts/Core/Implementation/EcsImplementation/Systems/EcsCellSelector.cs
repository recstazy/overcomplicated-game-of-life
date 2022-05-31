using GameOfLife.Abstraction;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace GameOfLife.Core.Ecs
{
    [DisableAutoCreation]
    public class EcsCellSelector : SystemBase, ICellSelector
    {
        private readonly IGameOfLifeInput input;
        private bool isHoldingPointerDown;
        private IGameConfiguration configuration;
        private Camera mainCamera;

        public EcsCellSelector(IGameOfLifeInput input, IGameConfiguration configuration)
        {
            this.input = input;
            this.configuration = configuration;
            mainCamera = Camera.main;
            World.DefaultGameObjectInjectionWorld.AddSystem(this);
            var group = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<SimulationSystemGroup>();
            group.AddSystemToUpdateList(this);
            group.SortSystems();

            input.OnPointerHoldChanged += PointerHoldChanged;
        }

        public void Dispose()
        {
            if (input != null)
            {
                input.OnPointerHoldChanged -= PointerHoldChanged;
            }
        }

        protected override void OnUpdate()
        {
            if (!isHoldingPointerDown)
                return;

            var cameraInfo = new CameraInfo(mainCamera);
            var cellSize = new float2(configuration.CellSize, configuration.CellSize);
            var mousePosition = input.CurrentPointerPosition.ToFloat2();

            var ray = cameraInfo.ScreenPointToRay(mousePosition);
            var planeOrigin = float3.zero;
            var planeNormal = new float3(0f, 0f, -1f);
            bool wasHit = MathExtensions.RaycastPlane(ray.origin, ray.direction, planeOrigin, planeNormal, out var mousePosOnGamePlane);

            if (!wasHit)
                return;

            Debug.DrawLine(Vector3.zero, mousePosOnGamePlane, Color.red);

            Entities.ForEach((ref Cell cell, in Translation translation) =>
            {
                var rect = new MathRect(cell.Position - cellSize * 0.5f, cellSize);

                if (rect.Contains(mousePosOnGamePlane.ToFloat2()))
                    cell.IsAlive = true;

            }).Schedule();
        }

        private void PointerHoldChanged(bool isDown)
        {
            isHoldingPointerDown = isDown;
        }
    }
}
