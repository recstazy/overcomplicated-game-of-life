using GameOfLife.Abstraction;
using System;
using System.Linq;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using Zenject;

namespace GameOfLife.Core.Ecs
{
    public class EcsGameImplementation : IGameImplementation
    {
        public IGameConfiguration Configuration { get; set; }
        private IsAliveSystem isAliveSystem;

        public void Initialize()
        {
            var gridSize = Configuration.GridSize;
            var world = World.DefaultGameObjectInjectionWorld;
            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            float cellSize = Configuration.CellSize;

            isAliveSystem = world.GetOrCreateSystem<IsAliveSystem>();
            isAliveSystem.SetGridSize(gridSize);
            world.GetOrCreateSystem<IsVisibleSystem>()
                .SetColors(Configuration.AliveColor.ToFloat4(), Configuration.DeadColor.ToFloat4());

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
                    entityManager.AddComponentData(entity, cell);
                    entityManager.AddComponentData(entity, new Scale() { Value = cellSize });
                    entityManager.AddComponentData(entity, new Translation() { Value = new float3(x * cellSize, y * cellSize, 0) });
                    entityManager.AddComponentData(entity, new UnlitColor() { Value = float4.zero });

                    var renderer = new RenderMesh();
                    renderer.material = Configuration.CellMaterial;
                    renderer.mesh = Configuration.CellMesh;
                    entityManager.AddSharedComponentData(entity, renderer);
                }
            }
        }

        public void Reset(Vector2Int[] newAlivePositions)
        {
            isAliveSystem.Reset(newAlivePositions.Select(x => x.ToInt2()).ToArray());
        }

        public void ScheduleUpdate() => isAliveSystem.ScheduleUpdate();

        public void Dispose() { }
    }
}
