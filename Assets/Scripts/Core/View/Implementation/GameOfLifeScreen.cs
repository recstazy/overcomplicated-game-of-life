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
        public event Action<float> OnResponseTimeChanged;

        [SerializeField]
        private TextMeshProUGUI isPlayingLabel;

        [SerializeField]
        private Slider intervalSlider;

        [SerializeField]
        private TextMeshProUGUI intervalValueLabel;

        [SerializeField]
        private Slider responseTimeSlider;

        [SerializeField]
        private TextMeshProUGUI responseTimeValueLabel;

        private IGameConfiguration configuration;
        private const string RESPONSE_TIME_FORMAT = "#0";

        [Inject]
        public void Construct(IGameConfiguration configuration)
        {
            this.configuration = configuration;
        }

        private void Awake()
        {
            var intervalSliderValue = Mathf.InverseLerp(configuration.MinUpdateInterval, configuration.MaxUpdateInterval, configuration.DefaultUpdateInterval);
            intervalSlider.SetValueWithoutNotify(intervalSliderValue);
            intervalValueLabel.text = configuration.DefaultUpdateInterval.ToString();
            intervalSlider.onValueChanged.AddListener(SpeedSliderValueChanged);

            var responseTimeSliderValue = Mathf.InverseLerp(configuration.MinPixelResponseTime, configuration.MaxPixelResponseTime, configuration.DefaultPixelResponseTime);
            responseTimeSlider.SetValueWithoutNotify(responseTimeSliderValue);
            responseTimeValueLabel.text = (configuration.DefaultPixelResponseTime * 1000f).ToString(RESPONSE_TIME_FORMAT);
            responseTimeSlider.onValueChanged.AddListener(ResponseTimeSliderValueChanged);
        }

        public void SetIsPlayting(bool isPlaying)
        {
            isPlayingLabel.text = isPlaying ? "Running" : "Paused";
        }

        private void SpeedSliderValueChanged(float newValue)
        {
            var intervalValue = Mathf.Lerp(configuration.MinUpdateInterval, configuration.MaxUpdateInterval, newValue);
            var roundedValue = Mathf.RoundToInt(intervalValue);
            OnUpdateIntervalChanged?.Invoke(roundedValue);
            intervalValueLabel.text = roundedValue.ToString();
        }

        private void ResponseTimeSliderValueChanged(float newValue)
        {
            var newResponseTime = Mathf.Lerp(configuration.MinPixelResponseTime, configuration.MaxPixelResponseTime, newValue);
            OnResponseTimeChanged?.Invoke(newResponseTime);
            responseTimeValueLabel.text = (newResponseTime * 1000f).ToString(RESPONSE_TIME_FORMAT);
        }
    }
}
