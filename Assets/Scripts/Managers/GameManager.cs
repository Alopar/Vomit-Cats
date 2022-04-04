using UnityEngine;
using UnityEditor;

namespace VomitCats
{
    public class GameManager : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [SerializeField] private float _gameTime;
        [SerializeField] private int _maxPollution;
        #endregion

        #region FIELDS PRIVATE
        private static GameManager _instance;

        private float _currentTime = 0;
        private int _currentPollution = 0;
        #endregion

        #region PROPERTIES
        public static GameManager Instance => _instance;
        #endregion

        #region UNITY CALLBACKS
        private void Awake()
        {
            if(_instance == null)
            {
                _instance = this;
            }
        }

        private void Start()
        {
            _currentTime = _gameTime;
            AudioManager.Play("GameBackground");
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameUiController.Instance.ShowPauseModal();
            }

            _currentTime -= Time.deltaTime;
            if(_currentTime <= 0)
            {
                GameUiController.Instance.ShowWinModal();
            }
        }
        #endregion

        #region METHODS PRIVATE
        #endregion

        #region METHODS PUBLIC
        public void AddPollution(int value)
        {
            _currentPollution += value;
            if(_currentPollution >= _maxPollution)
            {
                GameUiController.Instance.ShowLoseModal();
            }
        }

        public string GetTime()
        {
            var currentTime = 480f * (_currentTime / _gameTime);
            var hours = (int)currentTime / 60;
            var minuts = ((int)currentTime % 60).ToString("00");
            var timeString = $"{hours}:{minuts}";
            return timeString;
        }

        public string GetPollution()
        {
            var pollutionString = $"{_currentPollution}/{_maxPollution}";
            return pollutionString;
        }
        #endregion
    }
}
