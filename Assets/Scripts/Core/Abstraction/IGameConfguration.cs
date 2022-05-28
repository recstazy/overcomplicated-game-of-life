using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameOfLife.Abstraction
{
    public interface IGameConfguration
    {
        public int GridSize { get; }
        Mesh CellMesh { get; }
        Material CellMaterial { get; }
        Color AliveColor { get; }
        Color DeadColor { get; }
    }

    [Serializable]
    public class GameConfiguration : IGameConfguration
    {
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
    }
}
