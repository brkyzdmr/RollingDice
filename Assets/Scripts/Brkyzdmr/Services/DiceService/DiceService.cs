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
            
            GenerateDice(dicePrefab, diceCount);
            List<GameObject> diceList = GetGeneratedDiceObjects(diceCount);
            _animationRecorderService.StartSimulation(diceList);
            RecordDiceRollResults();
            _animationRecorderService.ResetToInitialState();
            SetTargetedResults();
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

        private List<GameObject> GetGeneratedDiceObjects(int diceCount)
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

        private void SetTargetedResults()
        {
            for (int i = 0; i < targetedResult.Count; i++)
            {
                diceDataList[i].diceLogic.RotateDice(((int)targetedResult[i]));
            }
        }

        private void GenerateDice(GameObject dicePrefab, int diceCount)
        {
            AdjustDiceListSize(diceCount);
            var randomStartPosition = _startPositions[Random.Range(0, _startPositions.Count)];
            SetDiceInitialState(diceCount, randomStartPosition);
        }

        private void AdjustDiceListSize(int diceCount)
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

        private void AddDice(int diceCount)
        {
            for (int i = 0; i < diceCount; i++)
            {
                var diceObject = _objectPoolService.Spawn("dice").Result;
                DiceData newDiceData = new DiceData(diceObject);
                diceDataList.Add(newDiceData);
            }
        }
        
        private void DisposeExtraDice(int diceToDispose)
        {
            diceToDispose = Mathf.Min(diceToDispose, diceDataList.Count);

            for (int i = 0; i < diceToDispose; i++)
            {
                var diceObject = diceDataList[^1].diceObject;
                _objectPoolService.Despawn("dice", diceObject);
                diceDataList.RemoveAt(diceDataList.Count - 1);
            }
        }


        private void SetDiceInitialState(int count, Transform startPosition)
        {
            int gridDimension = Mathf.CeilToInt(Mathf.Pow(count, 1.0f / 3.0f));
            List<Vector3> gridPositions = GridHelper.GenerateGridPositions(count, gridDimension, startPosition,
                diceDataList[0].rb.transform.localScale);

            for (int i = 0; i < count; i++)
            {
                InitialState initial = SetInitialState(diceDataList[i], gridPositions[i]);
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

        private InitialState SetInitialState(DiceData diceData, Vector3 position)
        {
            Quaternion rotation = MathHelper.GetRandomRotation();
            Vector3 force = PhysicsHelper.CalculateProjectileLaunchVelocity(position, Vector3.zero);
            Vector3 torque = MathHelper.GenerateRandomVector3(-15, 15);
            return new InitialState(position, rotation, force, torque);
        }
    }
}