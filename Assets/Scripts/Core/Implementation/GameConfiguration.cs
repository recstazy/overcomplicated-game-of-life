using GameOfLife.Abstraction;
using System;
using UnityEngine;

namespace GameOfLife.Core
{
    [Serializable]
    public class GameConfiguration : IGameConfiguration
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
        private int minUpdateInterval = 20;

        [SerializeField]
        private int maxUpdateInterval = 500;

        [SerializeField]
        private int defaultUpdateInterval = 40;

        public int GridSize => gridSize;
        public float CellSize => cellSize;
        public Mesh CellMesh => cellMesh;
        public Material CellMaterial => cellMaterial;
        public Color AliveColor => aliveColor;
        public Color DeadColor => deadColor;
        public GameImplementationType Implementation => implementationType;
        public int MinUpdateInterval => minUpdateInterval;
        public int MaxUpdateInterval => maxUpdateInterval;
        public int DefaultUpdateInterval => defaultUpdateInterval;
    }
}
