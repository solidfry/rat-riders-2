using UnityEngine;

namespace Events
{
    public class GameEvents : MonoBehaviour
    {
        public delegate void HealthChange(int currentHealth);
        public delegate void PlayerTakeDamage();
        public delegate void SetValue(int value);
        // public delegate void CurrentHealth(int health);
        public delegate void PlayerDied();
        public static HealthChange onHealthChangeEvent;
        public static PlayerDied onPlayerDiedEvent;
        public static SetValue onSetHealthCountEvent;
        public static PlayerTakeDamage onPlayerDamagedEvent;
    }
}