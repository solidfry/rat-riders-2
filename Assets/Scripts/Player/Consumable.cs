using Events;
using Interfaces;
using Rage;
using UnityEngine;

namespace Player
{
    public class Consumable : MonoBehaviour, IRage
    {
        [SerializeField] bool heal = false;
        [SerializeField] private int healValue = 1;
        [SerializeField] private RageValue rageValue = new();

        private void OnDestroy()
        {
            if (heal == true)
            {
                Debug.Log("Heal was invoked");
                GameEvents.onPlayerHealedEvent?.Invoke(healValue);
                GameEvents.onPlayerChangeRageEvent?.Invoke(rageValue.GetRageValue());
            }
        }

        public float GetRageValue()
        {
            return rageValue.GetRageValue();
        }
    }
}
