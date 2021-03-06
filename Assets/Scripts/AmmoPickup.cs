using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    [SerializeField] int ammoAmount = 5;
    [SerializeField] AmmoType ammoType;

    Ammo ammo;

    void Awake() 
    {
        ammo = FindObjectOfType<Ammo>();
    }

    void OnTriggerEnter(Collider other) 
    {
        if(other.tag == "Player")
        {
            ammo.IncreaseCurrentAmmo(ammoType, ammoAmount);
            Destroy(gameObject);
        }
    }
}
