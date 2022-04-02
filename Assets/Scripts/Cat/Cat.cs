using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace VomitCats
{
    public class Cat : MonoBehaviour
    {
        #region FIELDS INSPECTOR
        [Tooltip("Задержка рвоты")]
        [SerializeField, Range(0, 60)] private float _vomitDelay;
        [Tooltip("Продолжительность рвоты")]
        [SerializeField, Range(0, 20)] private float _vomitDuration;
        [Tooltip("Количество рвоты")]
        [SerializeField, Range(0, 5)] private int _vomitAmount;

        [Space(10)]
        [Tooltip("Скорость")]
        [SerializeField, Range(0, 10)] private float _speed;
        [Tooltip("Задержка блуждания")]
        [SerializeField, Range(0, 10)] private float _wanderDelay;
        [Tooltip("Радиус блуждания")]
        [SerializeField, Range(0, 5)] private float _wanderRadius;

        [Space(10)]
        [SerializeField] private Transform _body;
        [SerializeField] private Transform _shadow;

        [Space(10)]        
        [SerializeField] private Vomit _vomitPrefab;

        [Space(10)]
        [SerializeField] private Transform _startRoomPoint;
        #endregion

        #region FIELDS PRIVATE
        private CatState _state;
        private BaseStats _bodyBaseStats;
        private BaseStats _shadowBaseStats;

        private Transform _roomPoint;

        private bool _isOnHand;
        private Transform _handPlace;

        private float _waitVomitTimer;
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

            _roomPoint = _startRoomPoint;
        }

        private void Start()
        {
            SetVomitTimer();

            SetState(CatState.Idle);
        }

        private void Update()
        {
            CheckVomitTimer();

            if (_isOnHand)
            {
                transform.position = _handPlace.position;
            }
        }
        #endregion

        #region METHODS PRIVATE
        private void SetVomitTimer()
        {
            _waitVomitTimer = _vomitDelay;
        }

        private void CheckVomitTimer()
        {
            if (_state == CatState.Vomit) return;

            _waitVomitTimer -= Time.deltaTime;
            if (_waitVomitTimer < 0)
            {
                SetState(CatState.Vomit);
            }
        }

        private void ResetView()
        {
            _body.DOKill();
            _shadow.DOKill();

            _body.localScale = _bodyBaseStats.scale;
            _body.localPosition = _bodyBaseStats.position;

            _shadow.localScale = _shadowBaseStats.scale;
            _shadow.localPosition = _shadowBaseStats.position;
        }

        private void SetState(CatState state)
        {
            _state = state;

            ResetView();
            StopAllCoroutines();

            switch (_state)
            {
                case CatState.Idle:
                    _body.DOScaleY(0.8f, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Flash);
                    _body.DOLocalMoveY(0.4f, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Flash);

                    if (!_isOnHand)
                    {
                        StartCoroutine(Wander(_wanderDelay));
                    }
                    break;
                case CatState.Vomit:
                    _body.DOScale(new Vector3(0.7f, 0.7f, 1f), 0.5f).SetLoops(-1, LoopType.Restart).SetEase(Ease.Flash);
                    StartCoroutine(Vomit(_vomitDuration, _vomitAmount));
                    break;
                case CatState.Walk:
                    _body.DOLocalMoveY(0.8f, 0.3f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
                    _body.DOScaleX(0.8f, 0.3f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
                    _shadow.DOScale(new Vector3(0.5f, 0.5f, 1f), 0.3f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
                    break;
            }
        }

        private void CreateVomit()
        {
            var randomX = UnityEngine.Random.Range(-1f, 1f);
            var randomY = UnityEngine.Random.Range(-1f, 1f);
            var vomitPosition = new Vector3(transform.position.x + randomX, transform.position.y + randomY, transform.position.z);
            Instantiate(_vomitPrefab, vomitPosition, transform.rotation);
        }

        private void RandomMove()
        {
            var randomX = UnityEngine.Random.Range(0 - _wanderRadius, _wanderRadius);
            var randomY = UnityEngine.Random.Range(0 - _wanderRadius, _wanderRadius);
            var movePoint = new Vector3(_roomPoint.position.x + randomX, _roomPoint.position.y + randomY, transform.position.z);

            SetState(CatState.Walk);

            StartCoroutine(Move(movePoint));
        }
        #endregion

        #region METHODS PUBLIC
        public void TakeOnHand(Transform handPlace)
        {
            _isOnHand = true;
            _handPlace = handPlace;
            _shadow.gameObject.SetActive(false);

            if(_state != CatState.Vomit)
            {
                SetState(CatState.Idle);
            }
        }

        public void DropOnFloor(Vector3 place)
        {
            _isOnHand = false;            
            transform.position = place;
            _shadow.gameObject.SetActive(true);

            SetState(CatState.Idle);
        }
        #endregion

        #region COROUTINES
        private IEnumerator Vomit(float time, int number)
        {   
            var count = number;

            while(count > 0)
            {
                var timer = time;
                while(timer > 0)
                {
                    timer -= Time.deltaTime;
                    yield return null;
                }

                count--;
                CreateVomit();
            }

            SetVomitTimer();

            SetState(CatState.Idle);
        }

        private IEnumerator Wander(float time)
        {
            var timer = time;
            while(timer > 0)
            {
                timer -= Time.deltaTime;
                yield return null;
            }

            var randomRoll = UnityEngine.Random.Range(0, 100);
            if (randomRoll > 50)
            {
                RandomMove();
            }
            else
            {
                StartCoroutine(Wander(_wanderDelay));
            }
        }

        private IEnumerator Move(Vector3 point)
        {
            while(transform.position != point)
            {
                transform.position = Vector3.MoveTowards(transform.position, point, _speed * Time.deltaTime);
                yield return null;
            }

            SetState(CatState.Idle);
        }
        #endregion
    }
}