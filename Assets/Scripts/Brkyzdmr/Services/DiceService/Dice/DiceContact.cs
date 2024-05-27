
using Brkyzdmr.Services.EventService;
using Brkyzdmr.Services.ParticleService;
using Brkyzdmr.Services.SoundService;
using UnityEngine;
using UnityEngine.Serialization;

namespace Brkyzdmr.Services.DiceService
{
    public class DiceContact : MonoBehaviour
    {
        [Header("States")]
        public bool isContactWithFloor;
        public bool isContactWithDice;
        public bool isInSimulation = true;
        public bool isNotMoving = false;

        private IParticleService _particleService;
        private ISoundService _soundService;
        private IEventService _eventService;
        
        private bool _hasFloorContacted = false;
        private bool _hasDiceContacted = false;
        private bool _hasShownDiceResult = false;

        private void Awake()
        {
            _particleService = Services.GetService<IParticleService>();
            _soundService = Services.GetService<ISoundService>();
            _eventService = Services.GetService<IEventService>();
        }

        /// <summary>
        /// For a possible object pooling system,
        /// we could reset the dice back and reuse it again
        /// </summary>
        public void Reset()
        {
            isContactWithFloor = false;
            isContactWithDice = false;
            isInSimulation = true;
            isNotMoving = false;
            
            _hasFloorContacted = false;
            _hasDiceContacted = false;
            _hasShownDiceResult = false;
        }

        public void ShowDiceResult()
        {
            if (_hasShownDiceResult) return;
            _soundService.PlaySoundAndDespawn("dice_result_ding_sfx", transform.position);
            _hasShownDiceResult = true;
        }
    
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.CompareTag("Floor"))
            {
                isContactWithFloor = true;
                transform.GetComponent<Rigidbody>().velocity /= 1.2f;
            }
    
            if (collision.transform.CompareTag("Dice"))
            {
                isContactWithDice = true;
            }
        }
    
        private void OnCollisionStay(Collision collision)
        {
            if (collision.transform.CompareTag("Floor"))
            {
                isContactWithFloor = false;
            }
    
            if (collision.transform.CompareTag("Dice"))
            {
                isContactWithDice = false;
            }
        }
    
        private void OnCollisionExit(Collision collision)
        {
            if (collision.transform.CompareTag("Floor"))
            {
                isContactWithFloor = false;
            }
    
            if (collision.transform.CompareTag("Dice"))
            {
                isContactWithDice = false;
            }
        }

        public void FloorContacted()
        {
            if (_hasFloorContacted) return;
            _particleService.PlayParticle("dice_bounce_fx", transform.position - Vector3.down * 0.5f);
            _soundService.PlaySoundAndDespawn("dice_collide_floor_sfx", transform.position);
            _hasFloorContacted = true;
        }

        public void DiceContacted()
        {
            if (_hasDiceContacted) return;
            // _particleService.PlayParticle("dice_bounce_fx", transform.position - Vector3.down * 0.5f);
            _soundService.PlaySoundAndDespawn("dice_collide_dice_sfx", transform.position);
            _hasDiceContacted = true;
        }
    }
}
