using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace VomitCats
{   
    public class Vomit : MonoBehaviour, IPointerClickHandler
    {
        #region FIELDS INSPECTOR
        [Tooltip("Время разложения")]
        [SerializeField, Range(0, 60)] private float _decayDuration;

        [Space(10)]
        [SerializeField] private Transform _body;
        #endregion

        #region FIELDS PRIVATE
        private SpriteRenderer _spriteRenderer;

        private bool _isSeePlayer = false;
        private int _maxPollutionLevel;
        private int _currentPollutionLevel;
        #endregion

        #region EVENTS
        public event Action<Vomit> OnDecay;
        public event Action<Vomit> OnClear;
        #endregion

        #region UNITY CALLBACKS
        private void Awake()
        {
            _spriteRenderer = _body.GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            StartCoroutine(Decay(_decayDuration));            

            _maxPollutionLevel = GetPollutionLevel();
            _currentPollutionLevel = _maxPollutionLevel;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "PlayerVision")
            {
                _isSeePlayer = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag == "PlayerVision")
            {
                _isSeePlayer = false;
            }
        }

        private void OnDestroy()
        {
            _spriteRenderer.DOKill();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_isSeePlayer)
            {
                _currentPollutionLevel--;

                float currentScale = (float)_currentPollutionLevel / _maxPollutionLevel;

                _body.localScale = new Vector3(currentScale, currentScale, 1);

                AudioManager.Play("Clear");
                VfxManager.Instance.PlayVFX(transform.position, "Poof");

                if(_currentPollutionLevel <= 0)
                {
                    Clear();
                }
            }
        }
        #endregion

        #region METHODS PRIVATE
        private int GetPollutionLevel()
        {
            var pollutionLevel = 5;

            var collisions = Physics2D.OverlapPointAll(transform.position);
            foreach (var collision in collisions)
            {
                switch (collision.tag)
                {
                    case "EasyClean":
                        pollutionLevel = 3;
                        break;
                    case "HardClean":
                        pollutionLevel = 10;
                        break;
                }
            }

            return pollutionLevel;
        }
        #endregion

        #region METHODS PUBLIC
        public void Clear()
        {
            OnClear?.Invoke(this);

            VfxManager.Instance.PlayVFX(transform.position, "Firework");

            Destroy(gameObject);
        }
        #endregion

        #region COROUTINES
        private IEnumerator Decay(float time)
        {
            _spriteRenderer.DOColor(Color.black, _decayDuration);

            var timer = time;
            while(timer > 0)
            {
                timer -= Time.deltaTime;
                yield return null;
            }

            OnDecay?.Invoke(this);

            GameManager.Instance.AddPollution(1);

            Destroy(gameObject);
        }
        #endregion
    }
}
