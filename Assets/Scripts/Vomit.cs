using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

namespace VomitCats
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Vomit : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [Tooltip("Время разложения")]
        [SerializeField, Range(0, 60)] private float _decayDuration;
        #endregion

        #region FIELDS PRIVATE
        private SpriteRenderer _spriteRenderer;
        #endregion

        #region EVENTS
        public event Action<Vomit> OnDecay;
        public event Action<Vomit> OnClear;
        #endregion

        #region UNITY CALLBACKS
        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            StartCoroutine(Decay(_decayDuration));
            _spriteRenderer.DOColor(Color.grey, _decayDuration);
        }

        private void OnDestroy()
        {
            _spriteRenderer.DOKill();
        }
        #endregion

        #region METHODS PUBLIC
        public void Clear()
        {
            OnClear?.Invoke(this);

            Destroy(gameObject);
        }
        #endregion

        #region COROUTINES
        private IEnumerator Decay(float time)
        {
            var timer = time;
            while(timer > 0)
            {
                timer -= Time.deltaTime;
                yield return null;
            }

            OnDecay?.Invoke(this);

            Destroy(gameObject);
        }
        #endregion
    }
}
