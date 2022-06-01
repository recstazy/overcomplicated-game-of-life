using TMPro;
using UnityEngine;
using GameOfLife.Abstraction.View;

namespace GameOfLife.View
{
    public class GameOfLifeScreen : MonoBehaviour, IGameOfLifeScreen
    {
        [SerializeField]
        private TextMeshProUGUI isPlayingLabel;

        public void SetIsPlayting(bool isPlaying)
        {
            isPlayingLabel.text = isPlaying ? "Running" : "Paused";
        }
    }
}
