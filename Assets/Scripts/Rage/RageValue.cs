using System;
using Interfaces;
using UnityEngine;

namespace Rage
{
    [Serializable]
    public class RageValue : IRage
    {
       [SerializeField] private int value;

        public int Value
        {
            get => value;
            private set => this.value = value;
        }

        public int GetRageValue()
        {
            return Value;
        }
    }
}