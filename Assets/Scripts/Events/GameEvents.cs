using UnityEngine;

namespace Events
{
    public class GameEvents : MonoBehaviour
    {
        public delegate void HealthChange(int currentHealth);
        public delegate void PlayerTakeDamage();
        public delegate void SetValue(int value);
        public delegate void Heal();
        public delegate void PlayerDied();
        public delegate void ScreenShake(CameraShake.Strength str);
        public delegate void ObstacleSpawned(int count);

        public static HealthChange onHealthChangeEvent;
        public static PlayerDied onPlayerDiedEvent;
        public static SetValue onSetHealthCountEvent;
        public static PlayerTakeDamage onPlayerDamagedEvent;
        public static Heal onPlayerHealedEvent;
        public static ScreenShake onScreenShakeEvent;
        public static ObstacleSpawned onObstacleSpawnedEvent;
    }
}