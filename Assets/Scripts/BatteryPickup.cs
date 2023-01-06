using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryPickup : MonoBehaviour
{
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
            Destroy(gameObject);
        }
    }
}
