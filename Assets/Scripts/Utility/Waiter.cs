using System;
using System.Collections;
using UnityEngine;

namespace Utility
{
    /// <summary>
    /// Helper class for executing things with delay. Inspired by https://discussions.unity.com/t/how-to-wait-a-certain-amount-of-seconds-in-c/192244
    /// </summary>
    public class Waiter
    {
        /// <summary>
        /// Method to execute a function with delay
        /// </summary>
        /// <param name="seconds">Delay in seconds</param>
        /// <param name="callback">Function to execute</param>
        public IEnumerator WaitAndExecuteCoroutine(float seconds, Action callback)
        {
            yield return new WaitForSeconds(seconds);
            callback.Invoke();
        }
    }
}