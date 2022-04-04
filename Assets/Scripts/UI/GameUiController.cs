using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;
using DG.Tweening;
using TMPro;

namespace VomitCats
{
    public class GameUiController : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [SerializeField] private TextMeshProUGUI _timer;
        [SerializeField] private TextMeshProUGUI _pollution;
        [SerializeField] private TextMeshProUGUI _description;
        [SerializeField] private Image _descriptionBackground;

        [Space(10)]
        [SerializeField] private GameObject _startModal;
        [SerializeField] private TextMeshProUGUI _startModalText;
        [SerializeField] private string _enStartModalText;
        [SerializeField] private string _ruStartModalText;

        [Space(10)]
        [SerializeField] private GameObject _tutorialModal;
        [SerializeField] private TextMeshProUGUI _tutorialModalText;
        [SerializeField] private string _enTutorialModalText;
        [SerializeField] private string _ruTutorialModalText;

        [Space(10)]
        [SerializeField] private GameObject _winModal;
        [SerializeField] private TextMeshProUGUI _winModalText;
        [SerializeField] private string _enWinModalText;
        [SerializeField] private string _ruWinModalText;

        [Space(10)]
        [SerializeField] private GameObject _loseModal;
        [SerializeField] private TextMeshProUGUI _loseModalText;
        [SerializeField] private string _enLoseModalText;
        [SerializeField] private string _ruLoseModalText;

        [Space(10)]
        [SerializeField] private GameObject _pauseModal;        
        #endregion

        #region FIELDS PRIVATE
        private static GameUiController _instance;
        #endregion

        #region PROPERTIES
        public static GameUiController Instance => _instance;
        #endregion

        #region UNITY CALLBACKS
        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
        }

        private void Start()
        {
            switch (GameSettings.CurrentLanguage)
            {
                case Language.English:
                    _startModalText.text = _enStartModalText;
                    _tutorialModalText.text = _enTutorialModalText;
                    _winModalText.text = _enWinModalText;
                    _loseModalText.text = _enLoseModalText;
                    break;
                case Language.Russian:
                    _startModalText.text = _ruStartModalText;
                    _tutorialModalText.text = _ruTutorialModalText;
                    _winModalText.text = _ruWinModalText;
                    _loseModalText.text = _ruLoseModalText;
                    break;
            }

            PauseOn();
            RefreshTimer();
            RefreshPollution();

            _startModal.SetActive(true);
        }

        private void Update()
        {

            RefreshTimer();
            RefreshPollution();
        }
        #endregion

        #region METHODS PRIVATE
        #endregion

        #region METHODS PUBLIC
        public void SetDescription(string text)
        {
            _description.text = text;

            var startColor = new Color(_description.color.r, _description.color.g, _description.color.b, 1f);
            var startColorBG = new Color(_descriptionBackground.color.r, _descriptionBackground.color.g, _descriptionBackground.color.b, 0.5f);

            _description.color = startColor;
            _descriptionBackground.color = startColorBG;

            DOVirtual.Float(_description.color.a, 0f, 3f, (v) => { _description.color = new Color(startColor.r, startColor.g, startColor.b, v); }).SetEase(Ease.Linear);
            DOVirtual.Float(_descriptionBackground.color.a, 0f, 3f, (v) => { _descriptionBackground.color = new Color(startColorBG.r, startColorBG.g, startColorBG.b, v); }).SetEase(Ease.Linear);
        }

        public void PauseOn()
        {
            Time.timeScale = 0;
        }

        public void PauseOff()
        {
            Time.timeScale = 1f;
        }

        public void RefreshTimer()
        {
            _timer.text = $"Timer: {GameManager.Instance.GetTime()}";
        }

        public void RefreshPollution()
        {
            _pollution.text = $"Pollution: {GameManager.Instance.GetPollution()}";
        }

        public void RestartGame()
        {
            SceneManager.LoadScene("Game");
        }

        public void ExitGame()
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
        }

        public void ShowWinModal()
        {
            PauseOn();
            _winModal.SetActive(true);
        }

        public void ShowLoseModal()
        {
            PauseOn();
            _loseModal.SetActive(true);
        }

        public void ShowPauseModal()
        {
            PauseOn();
            _pauseModal.SetActive(true);
        }
        #endregion
    }
}
