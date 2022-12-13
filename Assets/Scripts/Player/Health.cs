using Events;
using UnityEngine;

namespace Player
{
    public class Health : MonoBehaviour
    {
        public int hitPoints;
        public int maxHitPoints = 3;
        public bool isPlayer;
        public int HitPoints
        {
            get => hitPoints;
            set
            {
                hitPoints = Mathf.Clamp(value, 0, maxHitPoints);
                if(isPlayer)
                {
                    GameEvents.onHealthChangeEvent?.Invoke(hitPoints);
                    if (hitPoints == 0) GameEvents.onPlayerDiedEvent?.Invoke();
                }
            }
        }

        private void Start() => GameEvents.onSetHealthCountEvent?.Invoke(HitPoints);

        private void OnEnable()
        {
            if(isPlayer)
            {
                GameEvents.onPlayerDamagedEvent += ReduceHitPoints;
                GameEvents.onPlayerHealedEvent += IncreaseHitPoints;
                GameEvents.onKillPlayerEvent += KillPlayer;
            }
        }
        
        private void OnDisable()
        {
            if(isPlayer)
            {
                GameEvents.onPlayerDamagedEvent -= ReduceHitPoints;
                GameEvents.onPlayerHealedEvent -= IncreaseHitPoints;
                GameEvents.onKillPlayerEvent -= KillPlayer;
            }
        }

        private void KillPlayer() => HitPoints = 0;


        private void ReduceHitPoints()
        {
            HitPoints--;
            if(isPlayer)
            {
                GameEvents.onScreenShakeEvent.Invoke(CameraShake.Strength.VeryHigh);
            }
        }
        private void IncreaseHitPoints(int value) => HitPoints += value;


    }
}