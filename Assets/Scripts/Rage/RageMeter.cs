using System;
using UnityEngine;

namespace Rage
{
    [Serializable]
    public class RageMeter
    {
        [Range(0,100)]
        [SerializeField] private int rageValue = 0;
        [SerializeField][ReadOnly] bool isRaging = false;

        public int RageValue
        {
            get => rageValue;
            set
            {
                rageValue = Mathf.Clamp(value, 0, 100);
                if(RageValue > 99)
                {
                    isRaging = true;
                }
                else
                {
                    isRaging = false;
                }
            }
        }
        
        public bool IsRaging
        {
            get => isRaging;
            set => isRaging = value;
        }

        public void AddRage(int rage) => RageValue += rage;

        public void ReduceRage(int rage) => RageValue -= rage;

        public void ResetRage() => RageValue = 0;
    
        public void ActivateRage()
        {
            IsRaging = true;
            RageValue = 100;
        }
    }
}
