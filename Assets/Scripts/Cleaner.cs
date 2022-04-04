using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

namespace VomitCats
{
    public class Cleaner : MonoBehaviour, IDrawSort
    {
        #region FIELDS INSPECTOR
        [Tooltip("Скорость")]
        [SerializeField, Range(0, 10)] private float _speed;
        [Tooltip("Задержка кошачего разговора")]
        [SerializeField, Range(0, 10)] private float _catTalkDelay;

        [Space(10)]
        [SerializeField] private Transform _body;
        [SerializeField] private Transform _shadow;
        [SerializeField] private Transform _handPlace;

        [Space(10)]
        [SerializeField] private Sprite _frontSprite;
        [SerializeField] private Sprite _backSprite;

        [Space(10)]        
        [SerializeField] private Talks _russianCatTalks;
        [SerializeField] private Talks _englishCatTalks;
        [SerializeField] private Message _messagePrefab;
        #endregion

        #region FIELDS PRIVATE
        private CleanerState _state;
        private BaseStats _bodyBaseStats;
        private BaseStats _shadowBaseStats;

        private Rigidbody2D _rigidbody;
        private SpriteRenderer _spriteRenderer;

        private Cat _cat;
        #endregion

        #region PROPERTIES
        public int DrawSortOrder;
        public HorizontalOrientation HorizontalOrientation;
        public VerticalOrientation VerticalOrientation;
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
            _spriteRenderer = _body.GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            SetState(CleanerState.Idle);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                DropCat();
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
                SetViewByDirection(direction);

                var currentPosition = transform.position;
                currentPosition += direction;

                _rigidbody.MovePosition(currentPosition);
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

            switch (_state)
            {
                case CleanerState.Idle:
                    _body.DOScaleY(0.95f, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Flash);
                    _body.DOLocalMoveY(0.65f, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Flash);
                    break;
                case CleanerState.Walk:
                    _body.DOLocalMoveY(0.8f, 0.3f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
                    //_body.DOScaleX(0.8f, 0.3f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
                    _shadow.DOScale(new Vector3(0.5f, 0.5f, 1f), 0.3f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
                    break;
            }
        }

        private void SetViewByDirection(Vector3 direction)
        {
            if (direction.x > 0)
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
                HorizontalOrientation = HorizontalOrientation.Right;
            }
            else
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
                HorizontalOrientation = HorizontalOrientation.Left;
            }

            if (direction.y <= 0)
            {
                _spriteRenderer.sprite = _frontSprite;
                VerticalOrientation = VerticalOrientation.Bottom;

            }
            else
            {
                _spriteRenderer.sprite = _backSprite;
                VerticalOrientation = VerticalOrientation.Up;
            }
        }
        #endregion

        #region METHODS PUBLIC
        public void SetCat(Cat cat)
        {
            if(_cat == null)
            {
                _cat = cat;
                cat.TakeOnHand(_handPlace);

                StartCoroutine(CatTalks(_catTalkDelay));
            }
        }

        public void DropCat()
        {
            if (_cat != null)
            {
                var dropPosition = new Vector3(transform.position.x, transform.position.y - 0.15f, transform.position.z);
                _cat.DropOnFloor(dropPosition);
                _cat = null;

                StopAllCoroutines();

                AudioManager.Play("Drop");
            }
        }

        public float GetPositionY()
        {
            return transform.position.y;
        }

        public void SetDrawOrder(int order)
        {
            _spriteRenderer.sortingOrder = order;
            DrawSortOrder = order;

            if(_cat != null)
            {
                _cat.SetDrawOrder(999);
            }
        }
        #endregion

        #region COROUTINES
        private IEnumerator CatTalks(float delay)
        {
            while (true)
            {
                var timer = delay;
                while(timer > 0)
                {
                    timer -= Time.deltaTime;
                    yield return null;
                }

                var messagePosition = transform.position;
                messagePosition.y += 2f;
                var message = Instantiate(_messagePrefab, messagePosition, _messagePrefab.transform.rotation);

                switch (GameSettings.CurrentLanguage)
                {
                    case Language.Russian:
                        message.SetText(_russianCatTalks.GetRandomMessage());
                        break;
                    case Language.English:
                        message.SetText(_englishCatTalks.GetRandomMessage());
                        break;
                }
            }
        }
        #endregion
    }
}
