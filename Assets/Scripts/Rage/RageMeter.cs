using System;
using Events;
using UnityEngine;

namespace Rage
{
    [Serializable]
    public class RageMeter
    {
        [Range(0, 1)]
        [SerializeField] private float rageValue = 0;
        [SerializeField] private float speedMultiplier = 1.1f;
        [SerializeField] private float jumpMultiplier = 1.5f;

        [SerializeField][ReadOnly] bool isRaging = false;

        public float RageValue
        {
            get => rageValue;
            set
            {
                rageValue = Mathf.Clamp(value, 0, 1);
                if (RageValue == 1)
                {
                    isRaging = true;
                }
                else
                {
                    isRaging = false;
                }
            }
        }

        public float SpeedMultiplier
        {
            get => speedMultiplier;
            set => speedMultiplier = value;
        }


        public float JumpMultiplier
        {
            get => jumpMultiplier;
            set => jumpMultiplier = value;
        }


        public bool IsRaging
        {
            get => isRaging;
            set => isRaging = value;
        }

        public void ChangeRage(float amount)
        {
            if (amount > 0)
            {
                AddRage(amount);
            }
            else
            {
                ReduceRage(amount);
            }
        }

        public void AddRage(float rageToAdd)
        {
            RageValue += rageToAdd;
            // Send the normalised value
            GameEvents.onChangeRageUIEvent?.Invoke(RageValue);
        }

        public void ReduceRage(float rageToReduce)
        {
            // if rageToReduce is negative make it positive
            var rageToReducePositive = rageToReduce < 0 ? rageToReduce * -1 : rageToReduce;
            RageValue -= rageToReducePositive;
            // Send the normalised value
            GameEvents.onChangeRageUIEvent?.Invoke(RageValue);
        }

        public void ResetRage()
        {
            RageValue = 0;
            // Send the normalised value
            GameEvents.onChangeRageUIEvent?.Invoke(RageValue);
        }

        public void ActivateRage()
        {
            IsRaging = true;
            RageValue = 100;
        }
    }
}
