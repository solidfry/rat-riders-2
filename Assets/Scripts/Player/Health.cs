using Events;
using UnityEngine;

namespace Player
{
    public class Health : MonoBehaviour
    {
        public int hitPoints;
        public int maxHitPoints = 3;

        public int HitPoints
        {
            get => hitPoints;
            set
            {
                hitPoints = Mathf.Clamp(value, 0, maxHitPoints);
                GameEvents.onHealthChangeEvent?.Invoke(hitPoints);
                if (hitPoints == 0) GameEvents.onPlayerDiedEvent?.Invoke();
            }
        }

        private void Start() => GameEvents.onSetHealthCountEvent?.Invoke(HitPoints);

        private void OnEnable()
        {
            GameEvents.onPlayerDamagedEvent += ReduceHitPoints;
            GameEvents.onPlayerHealedEvent += IncreaseHitPoints;
        }

        private void OnDisable()
        {
            GameEvents.onPlayerDamagedEvent -= ReduceHitPoints;
            GameEvents.onPlayerHealedEvent -= IncreaseHitPoints;

        }

        private void ReduceHitPoints()
        {
            HitPoints--;
            GameEvents.onScreenShakeEvent.Invoke(CameraShake.Strength.VeryHigh);
        }
        private void IncreaseHitPoints() => HitPoints++;


    }
}