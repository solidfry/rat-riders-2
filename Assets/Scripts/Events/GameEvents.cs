using UnityEngine;
using Utilities;

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
        public delegate void ScreenShake(CameraShake.Strength str, float lengthInSeconds = 0.2f);
        public delegate void ObstacleSpawned(int count);
        public delegate void PlayerChangeRage(float amount);
        public delegate void ChangeUIRage(float normalisedAmount);
        public delegate void LoadNextLevel();

        public static HealthChange onHealthChangeEvent;
        public static PlayerDied onPlayerDiedEvent;
        public static KillPlayer onKillPlayerEvent;
        public static SetValue onSetHealthCountEvent;
        public static PlayerTakeDamage onPlayerDamagedEvent;
        public static PlayerChangeRage onPlayerChangeRageEvent;
        public static ChangeUIRage onChangeRageUIEvent;
        public static Heal onPlayerHealedEvent;
        public static ScreenShake onScreenShakeEvent;
        public static ObstacleSpawned onObstacleSpawnedEvent;
        public static LoadNextLevel onLoadNextLevelEvent;
    }
}