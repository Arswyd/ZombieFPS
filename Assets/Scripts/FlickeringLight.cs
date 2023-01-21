using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
	[SerializeField] float period = 0.0f;
    [SerializeField] float amplitude = 1.0f;
    [SerializeField] float randomDeviation = 0.0f;

    [SerializeField] float randomDelay = 0.0f;

	Light mylight;
    float originalIntensity;

	void Start () 
    {   
		mylight = GetComponent<Light>();
        originalIntensity = mylight.intensity;
        randomDelay = Random.Range(-randomDelay, randomDelay);
	}

    void Update()
    {
        mylight.intensity = EvalWave();
    }

	float EvalWave () 
    {
        if (period <= Mathf.Epsilon) { return 0.0f; } // protect against period is zero
  
        float cycles = Time.time / period;

        const float tau = Mathf.PI * 2;
        float rawSinWave = Mathf.Sin(cycles * tau + randomDelay);
        
		return (rawSinWave * amplitude) + Random.Range(-randomDeviation, randomDeviation)  + originalIntensity;    
	}
}