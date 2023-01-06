using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessingHandler : MonoBehaviour
{

    [SerializeField] float maxVignetteIntensity = 0.5f;
    PostProcessVolume volume;
    Vignette vignette;
    bool showingVignette = false;

    void Awake()
    {
        volume = GetComponent<PostProcessVolume>();
        volume.profile.TryGetSettings(out vignette);
    }

    public void ShowDamageVignette()
    {
        showingVignette = true;
        vignette.intensity.value = maxVignetteIntensity;
    }

    void Update()
    {
        if (vignette.intensity.value == 0)
        {
            showingVignette = false;
        }
        if (showingVignette)
        {
            vignette.intensity.value -= Time.deltaTime;
        }
    }
}


