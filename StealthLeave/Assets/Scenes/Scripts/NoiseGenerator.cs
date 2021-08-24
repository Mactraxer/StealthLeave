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

    GameObject noiseDriverHolder;

    void Start()
    {
        noiseDriverHolder = new GameObject();
        noiseDriverHolder.AddComponent<NoiseDriver>();
        noiseDriverHolder.GetComponent<NoiseDriver>().SetupDriver(0, 5, 3, 1, 3, 1);
        noiseDriverHolder.GetComponent<NoiseDriver>().MakeSound += HandleDriverSound;
        noiseDriverHolder.GetComponent<NoiseDriver>().IncreasedNoiseValue += HandleIncreaseNoiseValue;
        startIncreaseNoise();


    }

    // Update is called once per frame
    void Update()
    {
        if (startPosition.position != transform.position)
        {
            noiseDriverHolder.GetComponent<NoiseDriver>().CurrentState = DriverNoiseState.move;
            Debug.Log(noiseDriverHolder.GetComponent<NoiseDriver>().CurrentState);
        } 
        else
        {
            noiseDriverHolder.GetComponent<NoiseDriver>().CurrentState = DriverNoiseState.idle;
            Debug.Log(noiseDriverHolder.GetComponent<NoiseDriver>().CurrentState);
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
        increaseTimer.Enabled = true;

    }

}
