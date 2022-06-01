using TMPro;
using UnityEngine;
using GameOfLife.Abstraction.View;
using System;
using UnityEngine.UI;
using Zenject;
using GameOfLife.Abstraction;

namespace GameOfLife.View
{
    public class GameOfLifeScreen : MonoBehaviour, IGameOfLifeScreen
    {
        public event Action<int> OnUpdateIntervalChanged;

        [SerializeField]
        private TextMeshProUGUI isPlayingLabel;

        [SerializeField]
        private Slider intervalSlider;

        [SerializeField]
        private TextMeshProUGUI intervalValueLabel;

        private IGameConfiguration configuration;

        [Inject]
        public void Construct(IGameConfiguration configuration)
        {
            this.configuration = configuration;
        }

        private void Awake()
        {
            var sliderValue = Mathf.InverseLerp(configuration.MinUpdateInterval, configuration.MaxUpdateInterval, configuration.DefaultUpdateInterval);
            intervalSlider.SetValueWithoutNotify(sliderValue);
            intervalValueLabel.text = configuration.DefaultUpdateInterval.ToString();
            intervalSlider.onValueChanged.AddListener(SpeedSliderValueChanged);
        }

        public void SetIsPlayting(bool isPlaying)
        {
            isPlayingLabel.text = isPlaying ? "Running" : "Paused";
        }

        private void SpeedSliderValueChanged(float newValue)
        {
            var clampedIntervalValue = ClampAndRoundInterval(newValue);
            OnUpdateIntervalChanged?.Invoke(clampedIntervalValue);
            intervalValueLabel.text = clampedIntervalValue.ToString();
        }

        private int ClampAndRoundInterval(float rawValue)
        {
            var intervalValue = Mathf.Lerp(configuration.MinUpdateInterval, configuration.MaxUpdateInterval, rawValue);
            return Mathf.RoundToInt(intervalValue);
        }
    }
}
