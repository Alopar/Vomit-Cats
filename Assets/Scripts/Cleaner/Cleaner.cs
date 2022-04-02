using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace VomitCats
{
    public class Cleaner : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [Tooltip("Скорость")]
        [SerializeField, Range(0, 10)] private float _speed;

        [Space(10)]
        [SerializeField] private Transform _body;
        [SerializeField] private Transform _shadow;
        [SerializeField] private Transform _handPlace;
        #endregion

        #region FIELDS PRIVATE
        private CleanerState _state;
        private BaseStats _bodyBaseStats;
        private BaseStats _shadowBaseStats;

        private Rigidbody2D _rigidbody;

        private Cat _catInHand;
        private float _keyDelay = 0;
        #endregion

        #region PROPERTIES
        #endregion

        #region EVENTS
        #endregion

        #region UNITY CALLBACKS
        private void Awake()
        {
            _bodyBaseStats = new BaseStats();
            _bodyBaseStats.scale = _body.localScale;
            _bodyBaseStats.position = _body.localPosition;

            _shadowBaseStats = new BaseStats();
            _shadowBaseStats.scale = _shadow.localScale;
            _shadowBaseStats.position = _shadow.localPosition;

            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            SetState(CleanerState.Idle);
        }

        private void Update()
        {
            _keyDelay -= _keyDelay > 0 ? Time.deltaTime : 0;

            if(_keyDelay <= 0)
            {
                if (_catInHand != null && Input.GetKey(KeyCode.E))
                {
                    _catInHand.DropOnFloor(transform.position);
                    _catInHand = null;

                    _keyDelay = 0.5f;
                }
            }
        }

        private void FixedUpdate()
        {
            if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
            {
                if (_state != CleanerState.Idle)
                {
                    SetState(CleanerState.Idle);
                }
            }
            else
            {
                if (_state != CleanerState.Walk)
                {
                    SetState(CleanerState.Walk);
                }

                var direction = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f);
                direction = Vector3.Normalize(direction) * _speed * Time.deltaTime;
                var currentPosition = transform.position;
                currentPosition += direction;

                _rigidbody.MovePosition(currentPosition);
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (_keyDelay > 0) return;
            if (_catInHand != null) return;

            var cat = collision.gameObject.GetComponent<Cat>();
            if (cat != null)
            {
                if (Input.GetKey(KeyCode.E))
                {
                    _catInHand = cat;
                    cat.TakeOnHand(_handPlace);

                    _keyDelay = 0.5f;
                }
            }
        }
        #endregion

        #region METHODS PRIVATE
        private void ResetView()
        {
            _body.DOKill();
            _shadow.DOKill();

            _body.localScale = _bodyBaseStats.scale;
            _body.localPosition = _bodyBaseStats.position;

            _shadow.localScale = _shadowBaseStats.scale;
            _shadow.localPosition = _shadowBaseStats.position;
        }

        private void SetState(CleanerState state)
        {
            _state = state;

            ResetView();
            StopAllCoroutines();

            switch (_state)
            {
                case CleanerState.Idle:
                    _body.DOScaleY(0.9f, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Flash);
                    _body.DOLocalMoveY(0.9f, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Flash);
                    break;
                case CleanerState.Walk:
                    _body.DOLocalMoveY(1.2f, 0.3f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
                    _body.DOScaleX(0.8f, 0.3f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
                    _shadow.DOScale(new Vector3(0.5f, 0.5f, 1f), 0.3f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
                    break;
            }
        }
        #endregion

        #region METHODS PUBLIC
        #endregion

        #region COROUTINES
        #endregion
    }
}
