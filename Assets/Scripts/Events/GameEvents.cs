using UnityEngine;

namespace Events
{
    public class GameEvents : MonoBehaviour
    {
        public delegate void PlayerDamaged();
        public static PlayerDamaged OnPlayerDamagedEvent;
    }
}