using System;
using UnityEngine;
using DG.Tweening;
using TMPro;

namespace VomitCats
{
    public class Message : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [SerializeField] private TextMeshProUGUI _text;
        #endregion

        #region METHODS PUBLIC
        public void SetText(string text)
        {
            _text.text = text;

            transform.DOMoveY(transform.position.y + 1f, 3f).OnComplete( () => Destroy(gameObject));;
            DOVirtual.Float(_text.color.a, 0f, 3f, (v) => { _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, v); }).SetEase(Ease.Linear);
        }
        #endregion
    }
}
