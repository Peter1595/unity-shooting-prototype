using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DelayEventManager : MonoBehaviour
{
    public void Wait(float delay, UnityAction action)
    {
        Debug.Log("delaying to action!");

        this.StartCoroutine(ExceuteAction(delay, action));
    }

    private IEnumerator ExceuteAction(float delay, UnityAction action)
    {
        Debug.Log("Waitng for: " + delay);

        yield return new WaitForSeconds(1f);

        Debug.Log("Invoke action");

        action.Invoke();
    }
}
