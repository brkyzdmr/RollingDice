using System.Collections.Generic;
using System.Linq;
using Brkyzdmr.Helpers;
using Brkyzdmr.Services.AnimationRecorderService;
using Brkyzdmr.Services.EventService;
using Brkyzdmr.Services.ObjectPoolService;
using RollingDice.Runtime.Event;
using UnityEngine;

namespace Brkyzdmr.Services.DiceService
{
    public class DiceService : Service, IDiceService
    {
        public int diceCount
        {
            get => _diceCount;
            set
            {
                _diceCount = value;
                targetedResult.Clear();
                Debug.Log("diceCount:" + diceCount);
                for (int i = 0; i < _diceCount; i++)
                {
                    targetedResult.Add(DiceValue.Any);
                }
            }
        }

        public List<DiceData> diceDataList { get; } = new();
        public List<DiceValue> targetedResult { get; } = new();
        private List<Transform> _startPositions;
        private IAnimationRecorderService _animationRecorderService;
        private readonly IEventService _eventService = Services.GetService<IEventService>();
        private IObjectPoolService _objectPoolService = Services.GetService<IObjectPoolService>();
        private int _diceCount;

        public void InitializeStartPositions(Transform startPositionRoot)
        {
            _startPositions = new List<Transform>();
            for (var i = 0; i < startPositionRoot.transform.childCount; i++)
            {
                _startPositions.Add(startPositionRoot.GetChild(i));
            }
        }

        public void RollTheDice(GameObject dicePrefab)
        {
            _animationRecorderService = Services.GetService<IAnimationRecorderService>();
            
            GenerateDice();
            List<GameObject> diceList = GetGeneratedDiceObjects();
            _animationRecorderService.StartSimulation(diceList);
            RecordDiceRollResults();
            _animationRecorderService.ResetToInitialState();
            SetTargetDiceValues();
            _animationRecorderService.PlayRecording(() =>
            {
                _eventService.Get<OnDiceRolled>().Execute(GetDiceResults());
            });
        }

        public List<int> GetDiceResults()
        {
            var diceResults = new List<int>();
            foreach (var diceData in diceDataList)
            {
                var diceResult = diceData.diceLogic.alteredFaceResult + 1;
                diceResults.Add(diceResult);
            }

            string dices = string.Join(", ", diceResults.Select(x => x.ToString()).ToArray());

            Debug.Log(dices + ": " + diceResults.Sum());
            return diceResults;
        }

        private List<GameObject> GetGeneratedDiceObjects()
        {
            List<GameObject> diceList = new List<GameObject>();
            for (int i = 0; i < diceCount; i++)
            {
                diceList.Add(diceDataList[i].diceObject);
            }
            return diceList;
        }

        private void RecordDiceRollResults()
        {
            for (int i = 0; i < diceCount; i++)
            {
                int result = diceDataList[i].diceLogic.FindFaceResult();
            }
        }

        private void SetTargetDiceValues()
        {
            for (int i = 0; i < targetedResult.Count; i++)
            {
                diceDataList[i].diceLogic.RotateDice(((int)targetedResult[i]));
            }
        }

        private void GenerateDice()
        {
            AdjustDiceListSize();
            var randomStartPosition = _startPositions[Random.Range(0, _startPositions.Count)];
            SetDiceInitialState(diceCount, randomStartPosition);
        }

        private void AdjustDiceListSize()
        {
            if (diceCount > diceDataList.Count)
            {
                AddDice(diceCount - diceDataList.Count);
            }
            else if (diceCount < diceDataList.Count)
            {
                DisposeExtraDice(diceDataList.Count - diceCount);
            }
        }

        private void AddDice(int diceToAddCount)
        {
            for (int i = 0; i < diceToAddCount; i++)
            {
                var diceObject = _objectPoolService.Spawn("dice").Result;
                DiceData newDiceData = new DiceData(diceObject);
                diceDataList.Add(newDiceData);
            }
        }
        
        private void DisposeExtraDice(int diceToDisposeCount)
        {
            diceToDisposeCount = Mathf.Min(diceToDisposeCount, diceDataList.Count);

            for (int i = 0; i < diceToDisposeCount; i++)
            {
                var diceObject = diceDataList[^1].diceObject;
                _objectPoolService.Despawn("dice", diceObject);
                diceDataList.RemoveAt(diceDataList.Count - 1);
            }
        }


        private void SetDiceInitialState(int totalDiceCount, Transform startPosition)
        {
            int gridDimension = Mathf.CeilToInt(Mathf.Pow(totalDiceCount, 1.0f / 3.0f));
            List<Vector3> gridPositions = GridHelper.GenerateGridPositions(totalDiceCount, gridDimension, startPosition,
                diceDataList[0].rb.transform.localScale);

            for (int i = 0; i < totalDiceCount; i++)
            {
                InitialState initial = SetInitialState(gridPositions[i]);
                diceDataList[i].diceLogic.Reset();
                diceDataList[i].diceContact.Reset();
                diceDataList[i].diceObject.transform.position = initial.position;
                diceDataList[i].diceObject.transform.rotation = initial.rotation;
                diceDataList[i].rb.useGravity = true;
                diceDataList[i].rb.isKinematic = false;
                diceDataList[i].rb.velocity = initial.force;
                diceDataList[i].rb.AddTorque(initial.torque, ForceMode.VelocityChange);
            }
        }

        private InitialState SetInitialState(Vector3 position)
        {
            Quaternion rotation = MathHelper.GetRandomRotation();
            Vector3 force = PhysicsHelper.CalculateProjectileLaunchVelocity(position, Vector3.zero);
            Vector3 torque = MathHelper.GenerateRandomVector3(-15, 15);
            return new InitialState(position, rotation, force, torque);
        }
    }
}