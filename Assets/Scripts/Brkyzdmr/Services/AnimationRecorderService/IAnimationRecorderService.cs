using System;
using System.Collections.Generic;
using UnityEngine;

namespace Brkyzdmr.Services.AnimationRecorderService
{
    public interface IAnimationRecorderService
    {
        void SetSimulationParameters(int frameLength, float simulationSpeed);
        void StartSimulation(List<GameObject> targets);
        void PlayRecording(Action onComplete = null);
        void ResetToInitialState();
    }
}