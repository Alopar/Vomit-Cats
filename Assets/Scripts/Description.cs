using UnityEngine;
using UnityEngine.EventSystems;


namespace VomitCats
{
    public class Description : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        #region FIELDS INSPECTOR
        [SerializeField] private string _ruDescription;
        [SerializeField] private string _enDescription;

        [Space(10)]
        [SerializeField] private GameObject _icon;
        #endregion

        #region UNITY CALLBACKS
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                switch (GameSettings.CurrentLanguage)
                {
                    case Language.Russian:
                        GameUiController.Instance.SetDescription(_ruDescription);
                        break;
                    case Language.English:
                        GameUiController.Instance.SetDescription(_enDescription);
                        break;
                }
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _icon.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _icon.SetActive(false);
        }
        #endregion
    }
}
