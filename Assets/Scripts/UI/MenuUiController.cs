using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;
using TMPro;


namespace VomitCats
{
    public class MenuUiController : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [SerializeField] private TextMeshProUGUI _languageButtonText;
        [SerializeField] private TextMeshProUGUI _muteButtonText;
        #endregion

        #region FIELDS PRIVATE
        #endregion

        #region UNITY CALLBACKS
        private void Start()
        {
            RefreshLanguageButton();
            RefreshMuteButton();

            AudioManager.Play("MenuBackground");
        }
        #endregion

        #region METHODS PRIVATE
        private void RefreshLanguageButton()
        {
            switch (GameSettings.CurrentLanguage)
            {
                case Language.English:
                    _languageButtonText.text = "English";
                    break;
                case Language.Russian:
                    _languageButtonText.text = "Русский";
                    break;
            }
        }

        private void RefreshMuteButton()
        {
            if (!GameSettings.Mute)
            {
                _muteButtonText.text = "Sound: On";
            }
            else
            {
                _muteButtonText.text = "Sound: Off";
            }
        }
        #endregion

        #region METHODS PUBLIC
        public void StartGame()
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

        public void ChangeLanguage()
        {
            if(GameSettings.CurrentLanguage == Language.English)
            {
                GameSettings.CurrentLanguage = Language.Russian;
            }
            else
            {
                GameSettings.CurrentLanguage = Language.English;
            }

            RefreshLanguageButton();
        }

        public void ChangeMute()
        {
            if (GameSettings.Mute)
            {
                GameSettings.Mute = false;
            }
            else
            {
                GameSettings.Mute = true;
            }

            AudioManager.ChangeMute(SoundType.Sound, GameSettings.Mute);
            AudioManager.ChangeMute(SoundType.Music, GameSettings.Mute);

            RefreshMuteButton();
        }
        #endregion
    }
}
