using System;
using System.Collections;
using System.Collections.Generic;
using Brkyzdmr.Helpers;
using Brkyzdmr.Services.CoroutineService;
using Brkyzdmr.Services.DiceService;
using Brkyzdmr.Services.EventService;
using Brkyzdmr.Services.ParticleService;
using UnityEngine;

namespace Brkyzdmr.Services.AnimationRecorderService
{
    public class AnimationRecorderService : Service, IAnimationRecorderService
    {
        private List<GameObject> _objectsToRecord;
        private List<RecordingData> _recordingDataList;
        private Coroutine _playback = null;

        private int _frameLength = 300;
        private float _simulationSpeed = 1f;
        private readonly ICoroutineService _coroutineService;
        private readonly IDiceService _diceService;

        public AnimationRecorderService()
        {
            _coroutineService = Services.GetService<ICoroutineService>();
            _diceService = Services.GetService<IDiceService>();

            _objectsToRecord = new List<GameObject>();
            _recordingDataList = new List<RecordingData>();
        }

        public void SetSimulationParameters(int frameLength, float simulationSpeed)
        {
            _frameLength = frameLength;
            _simulationSpeed = simulationSpeed;
        }
        public void StartSimulation(List<GameObject> targets)
        {
            if (_playback != null)
            {
                _coroutineService.StopCoroutine(_playback);
                _playback = null;
            }

            _recordingDataList.Clear();
            _objectsToRecord.Clear();
            _objectsToRecord = targets;

            EnablePhysics();
            GetInitialState();
            StartRecording();
        }

        public void PlayRecording(Action onComplete)
        {
            if (_playback == null && _recordingDataList.Count > 0)
            {
                _playback = _coroutineService.StartCoroutine(PlayAnimation(onComplete));
            }
        }

        public void ResetToInitialState()
        {
            for (int i = 0; i < _objectsToRecord.Count; i++)
            {
                _objectsToRecord[i].transform.position = _recordingDataList[i].initialPosition;
                _objectsToRecord[i].transform.rotation = _recordingDataList[i].initialRotation;
            }
        }

        private void GetInitialState()
        {
            foreach (var gameObject in _objectsToRecord)
            {
                Vector3 initialPosition = gameObject.transform.position;
                Quaternion initialRotation = gameObject.transform.rotation;

                Rigidbody rb = gameObject.GetComponent<Rigidbody>();
                rb.maxAngularVelocity = 30;
                // rb.velocity = Vector3.zero;

                RecordingData data = new RecordingData(rb, initialPosition, initialRotation);
                _recordingDataList.Add(data);
            }
        }
        
        private void StartRecording()
        {
            Physics.simulationMode = SimulationMode.Script;

            //Begin recording position and rotation for every frame
            for (int i = 0; i < _frameLength; i++)
            {
                //For every gameObject
                for (int j = 0; j < _objectsToRecord.Count; j++)
                {
                    Vector3 position = _objectsToRecord[j].transform.position;
                    Quaternion rotation = _objectsToRecord[j].transform.rotation;
                    bool isContactWithArena = _diceService.diceDataList[j].diceContact.isContactWithFloor;
                    bool isContactWithDice = _diceService.diceDataList[j].diceContact.isContactWithDice;
                    bool isNotMoving = PhysicsHelper.HasRigidbodyStopped(_diceService.diceDataList[j].rb);

                    RecordedFrame frame = new RecordedFrame(position, rotation, isContactWithArena, isContactWithDice,
                        isNotMoving);
                    _recordingDataList[j].recordedAnimation.Add(frame);
                }
                Physics.Simulate(Time.fixedDeltaTime * _simulationSpeed);
            }

            Physics.simulationMode = SimulationMode.FixedUpdate;
        }

        private IEnumerator PlayAnimation(Action onComplete)
        {
            DisablePhysics();
            ResetToInitialState();

            // Initialize a list to track indices of dice that are still moving
            List<int> activeDiceIndices = new List<int>();
            for (int j = 0; j < _recordingDataList.Count; j++)
            {
                activeDiceIndices.Add(j);
            }

            for (int i = 0; i < _frameLength; i++)
            {
                for (int k = activeDiceIndices.Count - 1; k >= 0; k--)
                {
                    int j = activeDiceIndices[k];
                    var animationData = _recordingDataList[j].recordedAnimation[i];

                    // Update object state
                    Vector3 position = animationData.position;
                    Quaternion rotation = animationData.rotation;
                    _objectsToRecord[j].SetActive(false);
                    _objectsToRecord[j].transform.position = position;
                    _objectsToRecord[j].transform.rotation = rotation;
                    _objectsToRecord[j].SetActive(true);

                    // Handle contacts
                    if (animationData.isContactWithArena)
                    {
                        _diceService.diceDataList[j].diceContact.FloorContacted();
                    }

                    if (animationData.isContactWithDice)
                    {
                        _diceService.diceDataList[j].diceContact.DiceContacted();
                    }

                    if (animationData.isNotMoving)
                    {
                        _diceService.diceDataList[j].diceContact.ShowDiceResult();
                        activeDiceIndices.RemoveAt(k); // Remove dice from the active list
                    }
                }

                // If no more active dice, break the loop
                if (activeDiceIndices.Count == 0)
                {
                    break;
                }

                yield return new WaitForFixedUpdate();
            }

            onComplete?.Invoke();
            _playback = null;
        }

        private void EnablePhysics()
        {
            for (int i = 0; i < _recordingDataList.Count; i++)
            {
                _recordingDataList[i].rb.useGravity = true;
                _recordingDataList[i].rb.isKinematic = false;
            }
        }

        private void DisablePhysics()
        {
            for (int i = 0; i < _recordingDataList.Count; i++)
            {
                _recordingDataList[i].rb.useGravity = false;
                _recordingDataList[i].rb.isKinematic = true;
            }
        }
    }
}