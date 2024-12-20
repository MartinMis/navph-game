using System;
using System.Collections;
using UnityEngine;

namespace Utility
{
    public class Waiter
    {
        
        public IEnumerator WaitAndExecuteCoroutine(float seconds, Action callback)
        {
            yield return new WaitForSeconds(seconds);
            callback.Invoke();
        }
    }
}