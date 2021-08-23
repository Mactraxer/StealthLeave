using System.Timers;
using System;

enum DriverNoiseState
{
    idle, move
}

class IncreaseNoiseTickEventArgs : EventArgs
{
    public float noiseValue { get; set; }
}

struct NoiseDriver
{
    private float noiseValue;
    private float noiseMax;
    private float decreaseNoiseTick;
    private float decreaseNoiseSpeed;
    private float increaseNoiseTick;
    private float increaseNoiseSpeed;

    private Timer decreaseTimer;
    private Timer increaseTimer;

    public NoiseDriver(float noiseValue, float noiseMax, float decreaseNoiseTick, float decreaseNoiseSpeed, float increaseNoiseTick, float increaseNoiseSpeed) : this()
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

        decreaseTimer = new Timer(decreaseNoiseTick * 1000f);
        decreaseTimer.Elapsed += decreaseNoise;
        decreaseTimer.AutoReset = true;
        decreaseTimer.Enabled = true;

    }

    private void decreaseNoise(System.Object source, ElapsedEventArgs e)
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

        increaseTimer = new Timer(increaseNoiseTick * 1000f);
        increaseTimer.Elapsed += increaseNoise;
        increaseTimer.AutoReset = true;
        increaseTimer.Enabled = true;

    }

    private void increaseNoise(System.Object source, ElapsedEventArgs e)
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
        if (decreaseTimer != null)
        {
            decreaseTimer.Stop();
            decreaseTimer.Dispose();
        }
        
    }

    private void stopIncreaseTimer()
    {
        if (increaseTimer != null)
        {
            increaseTimer.Stop();
            increaseTimer.Dispose();
        }
        
    }
}
