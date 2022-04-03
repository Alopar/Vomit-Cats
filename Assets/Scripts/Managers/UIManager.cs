using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace VomitCats
{
    public class UIManager : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [SerializeField] private TextMeshProUGUI _timer;
        [SerializeField] private TextMeshProUGUI _pollution;
        #endregion

        #region FIELDS PRIVATE
        #endregion

        #region UNITY CALLBACKS
        private void Start()
        {
            
        }

        private void Update()
        {
            _timer.text = $"Timer: {GameManager.Instance.GetTime()}";
            _pollution.text = $"Pollution: {GameManager.Instance.GetPollution()}";
        }
        #endregion

        #region METHODS PRIVATE
        #endregion

        #region METHODS PUBLIC
        #endregion
    }
}
