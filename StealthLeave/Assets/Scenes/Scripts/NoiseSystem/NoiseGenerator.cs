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
    Text noiseValueText;

    GameObject noiseDriverHolder;

    void Start()
    {
        noiseDriverHolder = new GameObject();
        noiseDriverHolder.AddComponent<NoiseDriver>();
        noiseDriverHolder.GetComponent<NoiseDriver>().SetupDriver(0, 4, 2, 1, 2, 1);
        noiseDriverHolder.GetComponent<NoiseDriver>().MakeSound += HandleDriverSound;
        noiseDriverHolder.GetComponent<NoiseDriver>().ChangeNoiseValue += HandleChangeNoiseValue;
    }

    // Update is called once per frame
    void Update()
    {
        if (startPosition.position != transform.position)
        {
            noiseDriverHolder.GetComponent<NoiseDriver>().CurrentState = DriverNoiseState.move;
        } 
        else
        {
            noiseDriverHolder.GetComponent<NoiseDriver>().CurrentState = DriverNoiseState.idle;
        }

    }

    void HandleChangeNoiseValue(object sender, ChangeNoiseTickEventArgs e)
    {
        noiseValueText.text = $"Уровень шума = {e.NoiseValue}";
    }

    void HandleDriverSound(object sender, EventArgs e)
    {
        noiseValueText.text = $"Вас обнаружили!!";
    }

}
