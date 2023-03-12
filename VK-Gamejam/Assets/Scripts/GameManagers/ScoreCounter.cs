using UnityEngine;
using UnityEngine.UI;

namespace GameManagers
{
    public class ScoreCounter : MonoBehaviour
    {
        [SerializeField] private Slider _slider;

        [SerializeField] private int _maxScore;

        private int _score;
        private static ScoreCounter _instance;

        public static ScoreCounter Instance { get { return _instance; } }

        private void Awake()
        {
            if (_instance != null && _instance != this)
                Destroy(this.gameObject);
            else
                _instance = this;

            _slider.maxValue = _maxScore;
            _score = 0;
            _slider.value = _score;
        }

        public void AddScore(int value)
        {
            _score += value;
            _slider.value = _score;
        }

        public void RemoveScore(int value)
        {
            _score -= value;
            _slider.value = _score;
        }
    }
}