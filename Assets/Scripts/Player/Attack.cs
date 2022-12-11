using Interfaces;
using Obstacles;
using Events;
using UnityEngine;

namespace Player
{
    public class Attack : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D col)
        {
            if(col.TryGetComponent(out IAttackable attackable))
            {
                attackable.TakeDamage();

                GameEvents.onScreenShakeEvent?.Invoke(CameraShake.Strength.Low);
            }
        }
    }
}
