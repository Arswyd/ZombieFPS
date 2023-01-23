using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryPickup : MonoBehaviour
{
    [SerializeField]  AudioClip clip;
    FlashlightSystem flashlightSystem;

    void Awake() 
    {
        flashlightSystem = FindObjectOfType<FlashlightSystem>();
    }
    
    void OnTriggerEnter(Collider other) 
    {
        if(other.tag == "Player")
        {
            flashlightSystem.RestoreBatteryCapacity();
            AudioSource.PlayClipAtPoint(clip, transform.position, 0.5f);
            Destroy(gameObject);
        }
    }
}
