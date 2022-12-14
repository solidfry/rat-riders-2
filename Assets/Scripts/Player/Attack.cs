using Interfaces;
using Events;
using UnityEngine;
using Utilities;

namespace Player
{
    public class Attack : MonoBehaviour
    {
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            if(col.TryGetComponent(out IAttackable attackable))
            {
                attackable.TakeDamage();

                GameEvents.onScreenShakeEvent?.Invoke(CameraShake.Strength.Low, .1f);
            }

            if (col.TryGetComponent(out IRage rage))
            {
                GameEvents.onPlayerChangeRageEvent?.Invoke(rage.GetRageValue());
                Debug.Log("Rage has been sent");
            }
        }
    }
}
