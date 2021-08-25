using System;
using UnityEngine;

/// <summary>
/// Класс работы с системой шума. Устанавливаем нужные параметры для набора значения шума и сброса и система с помошью таймеров следит за значением показателя шума и вызывает соответствующие ивенты
/// Так как класс работает с Timer то он наследуется от MonoBehaviour, чтобы держать зависить на Timer пришлость прибегнуть к костылю в виде GameObject к которому добавляю Timer как компонетн и так же работаю с Timer 
/// как с компонентом GameObject
/// </summary>
class NoiseDriver: MonoBehaviour
{
    private int noiseValue;
    private int noiseMax;
    private int decreaseNoiseTick;
    private int decreaseNoiseSpeed;
    private int increaseNoiseTick;
    private int increaseNoiseSpeed;

    private GameObject decreaseTimerHolder;
    private GameObject increaseTimerHolder;

    private DriverNoiseState currentState;
    public DriverNoiseState CurrentState
    {
        get { return currentState; }
        set
        {
            if (value != currentState)
            {
                StopDecreaseTimer();
                StopIncreaseTimer();


                switch (value)
                {
                    case DriverNoiseState.idle:
                        StartDecreaseNoise();
                        break;
                    case DriverNoiseState.move:
                        StartIncreaseNoise();
                        break;
                }
            }

            currentState = value;
        }
    }

    public event EventHandler MakeSound;
    public event EventHandler<ChangeNoiseTickEventArgs> ChangeNoiseValue;

    public void SetupDriver(int noiseValue, int noiseMax, int decreaseNoiseTick, int decreaseNoiseSpeed, int increaseNoiseTick, int increaseNoiseSpeed)
    {
        this.noiseValue = noiseValue;
        this.noiseMax = noiseMax;
        this.decreaseNoiseTick = decreaseNoiseTick;
        this.decreaseNoiseSpeed = decreaseNoiseSpeed;
        this.increaseNoiseTick = increaseNoiseTick;
        this.increaseNoiseSpeed = increaseNoiseSpeed;
        this.currentState = DriverNoiseState.idle;
    }

    private void InvokeChangeNoiseValueEvent()
    {
        ChangeNoiseTickEventArgs increaseNoiseEventArguments = new ChangeNoiseTickEventArgs();
        increaseNoiseEventArguments.NoiseValue = noiseValue;

        EventHandler<ChangeNoiseTickEventArgs> changeNoiseValueEvent = ChangeNoiseValue;
        if (changeNoiseValueEvent != null)
        {
            changeNoiseValueEvent(this, increaseNoiseEventArguments);
        }
    }

    private void StartDecreaseNoise()
    {
        if (decreaseTimerHolder == null)
        {
            decreaseTimerHolder = new GameObject();
            decreaseTimerHolder.AddComponent<Timer>();
            decreaseTimerHolder.GetComponent<Timer>().endTime = decreaseNoiseTick;
            decreaseTimerHolder.GetComponent<Timer>().IsRepeat = true;
            decreaseTimerHolder.GetComponent<Timer>().EndEvent += DecreaseNoise;
            decreaseTimerHolder.GetComponent<Timer>().IsEnable = true;
        } 
        else
        {
            decreaseTimerHolder.GetComponent<Timer>().IsEnable = true;
        }

    }

    private void DecreaseNoise(System.Object source, EventArgs e)
    {
        noiseValue -= decreaseNoiseSpeed;
        InvokeChangeNoiseValueEvent();
        if (noiseValue < 1)
        {
            StopDecreaseTimer();
        }
    }

    private void StartIncreaseNoise()
    {
        if (increaseTimerHolder == null)
        {
            increaseTimerHolder = new GameObject();
            increaseTimerHolder.AddComponent<Timer>();
            increaseTimerHolder.GetComponent<Timer>().endTime = increaseNoiseTick;
            increaseTimerHolder.GetComponent<Timer>().EndEvent += IncreaseNoise;
            increaseTimerHolder.GetComponent<Timer>().IsRepeat = true;
            increaseTimerHolder.GetComponent<Timer>().IsEnable = true;
        } 
        else
        {
            increaseTimerHolder.GetComponent<Timer>().IsEnable = true;
        }
    }

    private void IncreaseNoise(System.Object source, EventArgs e)
    {
        if (noiseValue < noiseMax)
        {
            noiseValue += increaseNoiseSpeed;
            InvokeChangeNoiseValueEvent();
        }
        else
        {
            MakeSound(this, EventArgs.Empty);
        }
    }

    private void StopDecreaseTimer()
    {
        if (decreaseTimerHolder != null)
        {
            decreaseTimerHolder.GetComponent<Timer>().IsEnable = false;
        }
    }

    private void StopIncreaseTimer()
    {
        if (increaseTimerHolder != null)
        {
            increaseTimerHolder.GetComponent<Timer>().IsEnable = false;
        }
    }

}
