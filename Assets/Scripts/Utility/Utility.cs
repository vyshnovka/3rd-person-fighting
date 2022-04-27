using System;
using System.Collections;
using UnityEngine;

public static class Utility
{
    public static IEnumerator TimedEvent(Action targetAction, float time)
    {
        yield return new WaitForSeconds(time);
        targetAction.Invoke();
    }
}
