using GameOfLife.Abstraction;
using System;
using UnityEngine;

namespace GameOfLife.Core
{
    [Serializable]
    public class GameConfiguration : IGameConfguration
    {
        [SerializeField]
        private GameImplementationType implementationType;

        [SerializeField]
        [Min(2)]
        private int gridSize = 5;

        [SerializeField]
        private Mesh cellMesh;

        [SerializeField]
        private Material cellMaterial;

        [SerializeField]
        private Color aliveColor = Color.white;

        [SerializeField]
        private Color deadColor = Color.black;

        public int GridSize => gridSize;
        public Mesh CellMesh => cellMesh;
        public Material CellMaterial => cellMaterial;
        public Color AliveColor => aliveColor;
        public Color DeadColor => deadColor;
        public GameImplementationType Implementation => implementationType;
    }
}
