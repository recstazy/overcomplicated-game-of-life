using GameOfLife.Abstraction;
using UnityEngine;

namespace GameOfLife
{
    [CreateAssetMenu(menuName = "Game Of Life/Scriptable Configuration", fileName = "GameConfiguration", order = 131)]
    public class ScriptableGameConfiguration : ScriptableObject, IGameConfiguration
    {
        [SerializeField]
        private GameImplementationType implementationType;

        [SerializeField]
        [Min(2)]
        private int gridSize = 5;

        [SerializeField]
        private float cellSize = 1f;

        [SerializeField]
        private Mesh cellMesh;

        [SerializeField]
        private Material cellMaterial;

        [SerializeField]
        private Color aliveColor = Color.white;

        [SerializeField]
        private Color deadColor = Color.black;

        [SerializeField]
        private RangedValue<int> updateInterval;

        [SerializeField]
        private RangedValue<float> pixelResponseTime;

        public int GridSize => gridSize;
        public float CellSize => cellSize;
        public Mesh CellMesh => cellMesh;
        public Material CellMaterial => cellMaterial;
        public Color AliveColor => aliveColor;
        public Color DeadColor => deadColor;
        public GameImplementationType Implementation => implementationType;
        public RangedValue<int> UpdateInterval => updateInterval;
        public RangedValue<float> PixelResponseTime => pixelResponseTime;
    }
}
