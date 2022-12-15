using System;
using Interfaces;
using UnityEngine;

namespace Rage
{
    [Serializable]
    public class RageValue : IRage
    {
        [Range(0, 1)]
        [SerializeField] private float value;

        public float Value
        {
            get => value;
            private set => this.value = value;
        }

        public float GetRageValue()
        {
            return Value;
        }
    }
}