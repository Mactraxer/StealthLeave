using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Таймер реализованный через coroutine поэтому пришлость унаследоваться от MonoBehaviour
/// </summary>
class Timer : MonoBehaviour
{
    int currentTime = 0;
    public int endTime;

    public event EventHandler EndEvent;
    public event EventHandler TickEvent;

    private bool isRepeat = false;
    public bool IsRepeat
    {
        get { return isRepeat; }
        set { isRepeat = value; }
    }

    private bool isEnable = false;
    public bool IsEnable 
    {
        get { return isEnable; }
        set
        {
            isEnable = value;
            if (isEnable == true)
            {
                StartCoroutine(startTimer());
            }
            else
            {
                StopCoroutine(startTimer());
                currentTime = 0;
            }
        }
    }

    private IEnumerator startTimer()
    {
        while (isRepeat)
        {
            if (isEnable == false) { break; }

            yield return new WaitForSeconds(endTime);
            EndEvent(this, EventArgs.Empty);
        }
    }

}
