using System.Collections;
using Events;
using UnityEngine;

namespace Core
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
                hitPoints = value;
                GameEvents.onHealthChangeEvent?.Invoke(hitPoints);
                if (hitPoints >= maxHitPoints) hitPoints = maxHitPoints;
                if (hitPoints == 0) GameEvents.onPlayerDiedEvent?.Invoke();
            }
        }

        private void Awake()
        {
            GameEvents.onSetHealthCountEvent?.Invoke(HitPoints);
        }

    }
}