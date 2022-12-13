using Rage;
using UnityEngine;

namespace Events
{
    public class GameEvents : MonoBehaviour
    {
        public delegate void HealthChange(int currentHealth);
        public delegate void PlayerTakeDamage();
        public delegate void SetValue(int value);
        public delegate void Heal(int value);
        public delegate void PlayerDied();

        public delegate void KillPlayer();
        public delegate void ScreenShake(CameraShake.Strength str);
        public delegate void ObstacleSpawned(int count);
        public delegate void PlayerChangeRage(int amount);

        public static HealthChange onHealthChangeEvent;
        public static PlayerDied onPlayerDiedEvent;
        public static KillPlayer onKillPlayerEvent;
        public static SetValue onSetHealthCountEvent;
        public static PlayerTakeDamage onPlayerDamagedEvent;
        public static PlayerChangeRage onPlayerChangeRageEvent;
        public static Heal onPlayerHealedEvent;
        public static ScreenShake onScreenShakeEvent;
        public static ObstacleSpawned onObstacleSpawnedEvent;
    }
}