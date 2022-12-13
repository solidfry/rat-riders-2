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
        
        public void ChangeRage(int amount)
        {
            if(amount > 0)
            {
                AddRage(amount);
            }
            else
            {
                ReduceRage(amount);
            }
        }

        public void AddRage(int rageToAdd) => RageValue += rageToAdd;

        public void ReduceRage(int rageToReduce)
        {
            // if rageToReduce is negative make it positive
            var rageToReducePositive = rageToReduce < 0 ? rageToReduce * -1 : rageToReduce;
            RageValue -= rageToReducePositive;
        }

        public void ResetRage() => RageValue = 0;
    
        public void ActivateRage()
        {
            IsRaging = true;
            RageValue = 100;
        }
    }
}
