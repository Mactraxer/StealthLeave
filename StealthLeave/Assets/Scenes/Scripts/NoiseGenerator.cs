using UnityEngine;
using System;
using UnityEngine.UI;

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

    [SerializeField]
    Text noiseValueText;

    NoiseDriver noiseDriver;

    void Start()
    {
        noiseDriver = new NoiseDriver(0f, 5f, 3f, 1f, 3f, 1f);
        noiseDriver.MakeSound += HandleDriverSound;
        noiseDriver.IncreasedNoiseValue += HandleIncreaseNoiseValue;
        startIncreaseNoise();


    }

    // Update is called once per frame
    void Update()
    {
        if (startPosition.position != transform.position)
        {
            noiseDriver.CurrentState = DriverNoiseState.move;
            Debug.Log(noiseDriver.CurrentState);
        } 
        else
        {
            noiseDriver.CurrentState = DriverNoiseState.idle;
            Debug.Log(noiseDriver.CurrentState);
        }

    }

    void HandleIncreaseNoiseValue(object sender, IncreaseNoiseTickEventArgs e)
    {
        noiseValueText.text = $"Уровень шума = {e.noiseValue}";
    }

    void HandleDriverSound(object sender, EventArgs e)
    {
        noiseValueText.text = $"Вас обнаружили!!";
    }
    // Для теста таймера 
    private System.Timers.Timer decreaseTimer;
    private System.Timers.Timer increaseTimer;

    private void startIncreaseNoise()
    {

        increaseTimer = new System.Timers.Timer(2000);
        increaseTimer.Elapsed += HandleDriverSound;
        increaseTimer.AutoReset = true;
        increaseTimer.Enabled = true;
        increaseTimer.Start();

    }

}
