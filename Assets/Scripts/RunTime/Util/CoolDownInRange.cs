using System;
using UnityEngine;

namespace Util
{
    /// <summary>
    /// This is a 
    /// </summary>
    public sealed class CoolDownInRange
    {
        public float timeUntilNextExecution;
        public float minimumCoolDownRange;
        public float maximumCoolDownRange;
        private float currentCoolDown;

        public Action onCoolDownReach;

        public CoolDownInRange(float _minimumCoolDownRange, float _maximumCoolDownRange)
        {
            timeUntilNextExecution = 0.0f;
            maximumCoolDownRange = _maximumCoolDownRange;
            minimumCoolDownRange = _minimumCoolDownRange;
            calculateCoolDown();
        }

        public void Update(float deltaTime)
        {
            currentCoolDown += deltaTime;

            if (currentCoolDown > timeUntilNextExecution)
            {
                currentCoolDown = 0.0f;
                calculateCoolDown();
                onCoolDownReach?.Invoke();
            }

        }
        private void calculateCoolDown()
        {
            timeUntilNextExecution = Mathf.Lerp(minimumCoolDownRange, maximumCoolDownRange, UnityEngine.Random.value);
        }


    }

}
