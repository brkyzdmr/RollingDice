using System;
using TMPro;
using UnityEngine;

namespace MatchemAll._Core.Scripts.Tools.Timer
{
    public class Timer : MonoBehaviour
    {
        [SerializeField] private TMP_Text timerText;
        
        public event Action OnCompleted;

        private float _countdownTimer;
        private float _time;
        private float _elapsedTime = 0f;
        private bool _doUpdate = true;

        private void Update()
        {
            if (!_doUpdate) { return; }
            
            _countdownTimer -= Time.deltaTime;
            _elapsedTime += Time.deltaTime; 
            
            if (_elapsedTime >= 1f)
            {
                SetTimerText();
                _elapsedTime = 0f;
            }
            
            if (_countdownTimer <= 0)
            {
                OnCompleted?.Invoke();
                _doUpdate = false;
            }
        }

        public void SetTimer(float time)
        {
            _countdownTimer = time;
            _time = time;
        }

        public void ResetTimer()
        {
            _countdownTimer = _time;
            _doUpdate = true;
        }

        public void StopTimer()
        {
            _doUpdate = false;
        }

        private void SetTimerText()
        {
            var time = GetRemainingTimeMinSecFormat();

            timerText.text = time;
        }
        
        private string GetRemainingTimeMinSecFormat()
        {
            int timer = (int)_countdownTimer;
            int minutes = timer / 60;
            int seconds = timer % 60;

            return $"{minutes:D2}:{seconds:D2}";
        }
    }
}