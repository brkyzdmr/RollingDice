using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Brkyzdmr.Attributes;
using Brkyzdmr.Extensions;
using Brkyzdmr.Services;
using Brkyzdmr.Services.CoroutineService;
using Brkyzdmr.Services.EventService;
using RollingDice.Runtime.Event;
using UnityEngine;

namespace RollingDice.Runtime.Board
{
    public class Avatar : MonoBehaviour
    {
        public Board board;

        [SerializeField] private int tilesToMove = -1; // -1 for infinite movement

        [Header("Jump")]
        [SerializeField] private float jumpHeight = 2f;

        [SerializeField] private float jumpDuration = 1f;
        [SerializeField] private AnimationCurve jumpRiseCurve;
        [SerializeField] private AnimationCurve jumpFallCurve;


        [Header("Tile Shake")] 
        [SerializeField] private float shakeDuration = 0.2f;

        [SerializeField] private float shakeIntensity = 0.3f;
        [SerializeField] private AnimationCurve shakeCurve;
        [SerializeField] private bool shakeRandomize = false;


        private ICoroutineService _coroutineService;
        private IEventService _eventService;
        private JumpHandler _jumpHandler;
        private int _currentTileIndex = 0;
        private int _direction = 1; // 1 for forward, -1 for backward
        private int _tilesMoved = 0;


        private void Awake()
        {
            _coroutineService = Services.GetService<ICoroutineService>();
            _eventService = Services.GetService<IEventService>();

            _jumpHandler = new JumpHandler(_coroutineService);
        }
        
        private void OnEnable()
        {
            _eventService.Get<OnDiceRolled>().AddListener(MoveThroughTiles);
        }

        private void OnDisable()
        {
            _eventService.Get<OnDiceRolled>().RemoveListener(MoveThroughTiles);
        }

        private void MoveThroughTiles(List<int> diceResults)
        {
            tilesToMove = diceResults.Sum();
            StartCoroutine(MoveThroughTilesCo());
        }

        private IEnumerator MoveThroughTilesCo()
        {
            while (tilesToMove == -1 || _tilesMoved < tilesToMove) // Check if we should keep moving
            {
                var currentTile = board.Tiles[_currentTileIndex].transform;
                int nextTileIndex = (_currentTileIndex + _direction + board.Tiles.Count) % board.Tiles.Count; // Ensure positive index
                var nextTile = board.Tiles[nextTileIndex].transform;

                bool isMoving = true;
                _jumpHandler.Jump(gameObject, transform.position, nextTile.position, jumpHeight, jumpDuration,
                    jumpRiseCurve, jumpFallCurve, () =>
                    {
                        isMoving = false;

                        nextTile.ShakeScale(shakeDuration, shakeIntensity, shakeCurve, shakeRandomize);
                        _currentTileIndex = nextTileIndex;
                    });

                yield return new WaitUntil(() => !isMoving);
                _tilesMoved++; // Increment the number of tiles moved
            }
            ResetTileCounter();
        }

        [Button("ChangeDirection")]
        public void ChangeDirection()
        {
            _direction *= -1; // Reverse the direction
        }
        
        private void ResetTileCounter()
        {
            _tilesMoved = 0;
        }
    }
}