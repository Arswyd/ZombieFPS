using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class FlashlightSystem : MonoBehaviour
{
    StarterAssetsInputs starterAssetsInputs;
    Light myLight;
    [SerializeField] float batteryCapacity = 10f;
    [SerializeField] float flickerDeviation = 1f;
    [SerializeField] float flickerLenght = 0.2f;
    float currentBatteryCapacity;
    bool isTurnedOn;
    float originalIntensity;
    bool isAbove60 = true;
    bool isAbove40 = true;
    bool isAbove10 = true;
    bool isFlickering;

    // Start is called before the first frame update
    void Awake()
    {
        myLight = GetComponent<Light>();
        starterAssetsInputs = FindObjectOfType<StarterAssetsInputs>();
        currentBatteryCapacity = batteryCapacity;
        originalIntensity = myLight.intensity;
    }

    // Update is called once per frame
    void Update()
    {
        ProcessKeyInput();
        ProcessBatteryUsage();
        if (isFlickering)
        {
            //myLight.intensity = originalIntensity + Mathf.Sin(Time.time * flickerFrequency) * flickerAmplitude;
            myLight.intensity = originalIntensity + Random.Range(-flickerDeviation, flickerDeviation);
        }
    }

    void ProcessBatteryUsage()
    {
        if (isTurnedOn && currentBatteryCapacity >= 0)
        {
            currentBatteryCapacity -= Time.deltaTime;
        }
        if (isTurnedOn && currentBatteryCapacity < 0)
        {
            isTurnedOn = false;
            myLight.enabled = false;
        }
        if (isTurnedOn && isAbove60 && currentBatteryCapacity/batteryCapacity < 0.6)
        {
            StartCoroutine(BeginFlickering(1f));
        }
        if (isTurnedOn && isAbove40 && currentBatteryCapacity/batteryCapacity < 0.4)
        {
            StartCoroutine(BeginFlickering(2f));
        }
        if (isTurnedOn && isAbove10 && currentBatteryCapacity/batteryCapacity < 0.1)
        {
            StartCoroutine(BeginFlickering(3f));
        }
    }

    public void RestoreBatteryCapacity()
    {
        currentBatteryCapacity = batteryCapacity;
        isAbove60 = true;
        isAbove40 = true;
        isAbove10 = true;
    }

    void ProcessKeyInput()
    {
        if(starterAssetsInputs.flashlight)
        {
            if(currentBatteryCapacity > 0)
            {
                isTurnedOn = !isTurnedOn;
                myLight.enabled = isTurnedOn;
            }
            starterAssetsInputs.flashlight = false;
        }
    }

    IEnumerator BeginFlickering(float flickerLenghtMultiplier)
    {
        if(flickerLenghtMultiplier == 1f)
            isAbove60 = false;
        if(flickerLenghtMultiplier == 2f)
            isAbove40 = false;
        if(flickerLenghtMultiplier == 3f)
            isAbove10 = false;

        isFlickering = true;

        yield return new WaitForSeconds(flickerLenght * flickerLenghtMultiplier);

        isFlickering = false;
        myLight.intensity = originalIntensity;
    }
}
