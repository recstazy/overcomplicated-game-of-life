using GameOfLife.Core.Ecs;
using Unity.Transforms;
using Unity.Rendering;
using UnityEngine;
using Unity.Entities;
using System.Threading.Tasks;
using Unity.Mathematics;
using System;

namespace GameOfLife.Core
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField]
        [Min(0)]
        private int updateLoopDelay;

        [SerializeField]
        [Min(2)]
        private int gridSize = 5;

        [SerializeField]
        private bool isPaused;

        [SerializeField]
        private Mesh cellMesh;

        [SerializeField]
        private Material cellMaterial;

        [SerializeField]
        private Color aliveColor = Color.white;

        [SerializeField]
        private Color deadColor = Color.black;

        [SerializeField]
        private int2[] aliveOnStartIndices;

        private IsAliveSystem isAliveSystem;

        private void Awake()
        {
            var world = World.DefaultGameObjectInjectionWorld;
            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            isAliveSystem = world.GetOrCreateSystem<IsAliveSystem>();
            isAliveSystem.SetGridSize(gridSize);
            world.GetOrCreateSystem<IsVisibleSystem>().SetColors(ToFloat4(aliveColor), ToFloat4(deadColor));

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
                    cell.IsAlive = Array.IndexOf(aliveOnStartIndices, cell.Position) >= 0;
                    entityManager.AddComponentData(entity, cell);
                    entityManager.AddComponentData(entity, new Scale() { Value = 1f });
                    entityManager.AddComponentData(entity, new Translation() { Value = new float3(x, y, 0) });
                    entityManager.AddComponentData(entity, new UnlitColor() { Value = float4.zero });

                    var renderer = new RenderMesh();
                    renderer.material = cellMaterial;
                    renderer.mesh = cellMesh;
                    entityManager.AddSharedComponentData(entity, renderer);
                }
            }
            
            StartUpdateLoop();
        }

        private async void StartUpdateLoop()
        {
            while (Application.isPlaying)
            {
                if (!isPaused)
                    isAliveSystem.ScheduleUpdate();

                await Task.Delay(updateLoopDelay);
            }
        }

        private float4 ToFloat4(Color color)
        {
            return new float4(color.r, color.g, color.b, color.a);
        }
    }
}
