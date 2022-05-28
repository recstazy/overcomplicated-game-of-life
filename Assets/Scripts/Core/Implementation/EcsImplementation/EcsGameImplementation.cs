using GameOfLife.Abstraction;
using System;
using System.Linq;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using Zenject;
using UnityEngine.InputSystem;

namespace GameOfLife.Core.Ecs
{
    public class EcsGameImplementation : IGameImplementation
    {
        public IGameConfguration Configuration { get; set; }
        private IsAliveSystem isAliveSystem;
        private IGameOfLifeInput input;

        [Inject]
        public void Construct(IGameOfLifeInput input)
        {
            this.input = input;
            input.OnPointerHoldChanged += PointerHoldChanged;
            input.OnPointerDrag += PointerDrag;
        }

        public void Initialize(Vector2Int[] aliveOnStart)
        {
            var gridSize = Configuration.GridSize;
            var world = World.DefaultGameObjectInjectionWorld;
            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            var aliveOnStartPositions = aliveOnStart.Select(x => ToInt2(x)).ToArray();
            isAliveSystem = world.GetOrCreateSystem<IsAliveSystem>();
            isAliveSystem.SetGridSize(gridSize);
            world.GetOrCreateSystem<IsVisibleSystem>().SetColors(ToFloat4(Configuration.AliveColor), ToFloat4(Configuration.DeadColor));

            var archetype = entityManager.CreateArchetype(
                typeof(Cell),
                typeof(Translation),
                typeof(Scale),
                typeof(RenderMesh),
                typeof(RenderBounds),
                typeof(LocalToWorld),
                typeof(UnlitColor));

            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    var entity = entityManager.CreateEntity(archetype);

                    var cell = new Cell(new int2(x, y));
                    cell.IsAlive = Array.IndexOf(aliveOnStartPositions, cell.Position) >= 0;
                    entityManager.AddComponentData(entity, cell);
                    entityManager.AddComponentData(entity, new Scale() { Value = 1f });
                    entityManager.AddComponentData(entity, new Translation() { Value = new float3(x, y, 0) });
                    entityManager.AddComponentData(entity, new UnlitColor() { Value = float4.zero });

                    var renderer = new RenderMesh();
                    renderer.material = Configuration.CellMaterial;
                    renderer.mesh = Configuration.CellMesh;
                    entityManager.AddSharedComponentData(entity, renderer);
                }
            }
        }

        public void ScheduleUpdate() => isAliveSystem.ScheduleUpdate();

        public void Dispose()
        {
            if (input != null)
            {
                input.OnPointerHoldChanged -= PointerHoldChanged;
                input.OnPointerDrag -= PointerDrag;
            }
        }

        private void PointerHoldChanged(bool isDown)
        {
            Debug.Log(isDown);
        }

        private void PointerDrag(Vector2Int newPosition)
        {
            Debug.Log(newPosition);
        }

        private float4 ToFloat4(Color color) 
            => new float4(color.r, color.g, color.b, color.a);

        private int2 ToInt2(Vector2Int vector)
            => new int2(vector.x, vector.y);
    }
}
