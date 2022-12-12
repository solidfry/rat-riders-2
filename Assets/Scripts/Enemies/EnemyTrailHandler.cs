using System;
using UnityEngine;

namespace Enemies
{
    [Serializable]
    public class EnemyTrailHandler
    {
        [SerializeField] private TrailRenderer trail;
        
        public void DisableTrail() => trail.enabled = false;
    }
}