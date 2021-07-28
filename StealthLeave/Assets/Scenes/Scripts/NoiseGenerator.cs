using UnityEngine;
using System.Timers;

enum DriverNoiseState
{
    idle, move
}

struct NoiseDriver
{
    float noiseValue;
    float decreaseNoiseTick;
    float decreaseNoiseSpeed;
    float increaseNoiseTick;
    float increaseNoiseSpeed;

    Timer decreaseTimer;
    Timer increaseTimer;
    DriverNoiseState currentState
    {
        get { return currentState; }
        set { 
            if (value != currentState)
            {
                decreaseTimer.Stop();
                decreaseTimer.Dispose();
                increaseTimer.Stop();
                increaseTimer.Dispose();

                switch (value)
                {
                    case DriverNoiseState.idle:
                        startDecreaseNoise();
                        break;
                    case DriverNoiseState.move:
                        startIncreaseNoise();
                        break;
                }
            } else
            {

            }
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
    }

    private void startIncreaseNoise()
    {
        increaseTimer = new Timer(decreaseNoiseTick * 1000f);
        increaseTimer.Elapsed += increaseNoise;
        increaseTimer.AutoReset = true;
        increaseTimer.Enabled = true;
    }

    private void increaseNoise(System.Object source, ElapsedEventArgs e)
    {
        noiseValue += increaseNoiseSpeed;
    }
}

public class NoiseGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    Transform startPosition;

    [SerializeField]
    Transform currentPosition;

    [SerializeField]
    float timeToSilence = 4f;
    [SerializeField]
    float currentIdleTime;

    [SerializeField]
    int noiseValue;

    [SerializeField]
    int maxNoiseValue;

    DriverNoiseState currentState;

    void Start()
    {
        currentState = DriverNoiseState.idle;
    }

    // Update is called once per frame
    void Update()
    {
        if (startPosition.position != transform.position)
        {
            currentState = DriverNoiseState.move;
        } else
        {
            currentState = DriverNoiseState.idle;
        }

    }


}
