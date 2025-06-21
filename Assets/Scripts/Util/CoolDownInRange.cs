using System;
using UnityEngine;

namespace Util
{
    /// <summary>
    /// This is a 
    /// </summary>
    public sealed class CoolDownInRange
    {
        public float currentTime;
        public float minimumCoolDownRange;
        public float maximumCoolDownRange;
        private float currentCoolDown;

        public Action onCoolDownReach;

        public CoolDownInRange(float _minimumCoolDownRange, float _maximumCoolDownRange)
        {
            currentTime = 0.0f;
            maximumCoolDownRange = _maximumCoolDownRange;
            minimumCoolDownRange = _minimumCoolDownRange;
            calculateCoolDown();
        }

        private void calculateCoolDown()
        {
            currentCoolDown = Mathf.Lerp(minimumCoolDownRange, maximumCoolDownRange, UnityEngine.Random.value);
        }


        private void Update(float deltaTime)
        {
            currentCoolDown += deltaTime;

            if (currentCoolDown > currentTime)
            {
                currentCoolDown = 0.0f;
                calculateCoolDown();
                onCoolDownReach?.Invoke();
            }

        }


    }

}
