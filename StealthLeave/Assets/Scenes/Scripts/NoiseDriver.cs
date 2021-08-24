using System.Timers;
using System;
using UnityEngine;

enum DriverNoiseState
{
    idle, move
}

class IncreaseNoiseTickEventArgs : EventArgs
{
    public float noiseValue { get; set; }
}

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

    //private System.Timers.Timer decreaseTimer;
    //private System.Timers.Timer increaseTimer;

    private Timer decreaseTimer;
    private Timer increaseTimer;
    private GameObject decreaseTimerHolder;
    private GameObject increaseTimerHolder;

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

    private DriverNoiseState currentState;
    public DriverNoiseState CurrentState
    {
        get { return currentState; }
        set
        {
            if (value != currentState)
            {
                stopDecreaseTimer();
                stopIncreaseTimer();


                switch (value)
                {
                    case DriverNoiseState.idle:
                        startDecreaseNoise();
                        break;
                    case DriverNoiseState.move:
                        startIncreaseNoise();
                        break;
                }
            }
            else
            {

            }

            currentState = value;
        }
    }

    public event EventHandler MakeSound;
    public event EventHandler<IncreaseNoiseTickEventArgs> IncreasedNoiseValue;

    private void invokeIncreasedNoiseValueEvent()
    {
        IncreaseNoiseTickEventArgs increaseNoiseEventArguments = new IncreaseNoiseTickEventArgs();
        increaseNoiseEventArguments.noiseValue = noiseValue;

        EventHandler<IncreaseNoiseTickEventArgs> increaseNoiseValueEvent = IncreasedNoiseValue;
        if (increaseNoiseValueEvent != null)
        {
            increaseNoiseValueEvent(this, increaseNoiseEventArguments);
        }
    }
    private void startDecreaseNoise()
    {
        if (decreaseTimerHolder == null)
        {
            decreaseTimerHolder = new GameObject();
            decreaseTimerHolder.AddComponent<Timer>();
            decreaseTimerHolder.GetComponent<Timer>().endTime = decreaseNoiseTick;
            decreaseTimerHolder.GetComponent<Timer>().IsRepeat = true;
            decreaseTimerHolder.GetComponent<Timer>().EndEvent += decreaseNoise;
            decreaseTimerHolder.GetComponent<Timer>().IsEnable = true;
        } 
        else
        {
            decreaseTimerHolder.GetComponent<Timer>().IsEnable = true;
        }

    }

    private void decreaseNoise(System.Object source, EventArgs e)
    {

        noiseValue -= decreaseNoiseSpeed;
        invokeIncreasedNoiseValueEvent();
        if (noiseValue < 1)
        {
            stopDecreaseTimer();
        }

    }

    private void startIncreaseNoise()
    {
        if (increaseTimerHolder == null)
        {
            increaseTimerHolder = new GameObject();
            increaseTimerHolder.AddComponent<Timer>();
            increaseTimerHolder.GetComponent<Timer>().endTime = increaseNoiseTick;
            increaseTimerHolder.GetComponent<Timer>().EndEvent += increaseNoise;
            increaseTimerHolder.GetComponent<Timer>().IsRepeat = true;
            increaseTimerHolder.GetComponent<Timer>().IsEnable = true;
        } 
        else
        {
            increaseTimerHolder.GetComponent<Timer>().IsEnable = true;
        }
        

    }

    private void increaseNoise(System.Object source, EventArgs e)
    {

        if (noiseValue < noiseMax)
        {
            noiseValue += increaseNoiseSpeed;
            invokeIncreasedNoiseValueEvent();
        }
        else
        {
            MakeSound.Invoke(this, EventArgs.Empty);
        }

    }

    private void stopDecreaseTimer()
    {
        if (decreaseTimerHolder != null)
        {
            decreaseTimerHolder.AddComponent<Timer>().IsEnable = false;
        }
        
    }

    private void stopIncreaseTimer()
    {
        if (increaseTimerHolder != null)
        {
            increaseTimerHolder.GetComponent<Timer>().IsEnable = false;
        }
        
    }
}
