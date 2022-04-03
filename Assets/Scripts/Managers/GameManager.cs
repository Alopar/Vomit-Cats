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
        private float _currentTime = 0;
        private int _currentPollution = 0;
        private Language _currentLanguage = Language.Russian;
        #endregion

        #region PROPERTIES
        public static GameManager Instance;
        public Language Language => _currentLanguage;
        #endregion

        #region UNITY CALLBACKS
        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
        }

        private void Start()
        {
            _currentTime = _gameTime;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
#if UNITY_EDITOR
                EditorApplication.ExitPlaymode();
#else
                Application.Quit();
#endif
            }

            _currentTime -= Time.deltaTime;
        }
        #endregion

        #region METHODS PRIVATE
        private void GameOver()
        {
            print("Game over!");
        }
        #endregion

        #region METHODS PUBLIC
        public void AddPollution(int value)
        {
            _currentPollution += value;

            if(_currentPollution >= _maxPollution)
            {
                GameOver();
            }
        }

        public string GetTime()
        {
            var currentTime = 480f * (_currentTime / _gameTime);
            var timeString = $"{(int)currentTime / 60}:{(int)currentTime % 60}";
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
