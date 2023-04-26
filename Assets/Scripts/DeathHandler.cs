using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeathHandler : MonoBehaviour
{
    [SerializeField] Canvas gameOverCanvas;
    [SerializeField] TextMeshProUGUI gameOverText;

    bool isStopped = false;

    void Start() 
    {
        gameOverCanvas.enabled = false;
    }

    public void HandleDeath()
    {
        SetIsStopped(true);
        gameOverCanvas.enabled = true;
        Time.timeScale = 0;
        FindObjectOfType<WeaponSwitcher>().enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Finish")
        {
            gameOverText.text = "You escaped";
            HandleDeath();
        }     
    }

    public bool GetIsStopped()
    {
        return isStopped;
    }

    public void SetIsStopped(bool _isStopped)
    {
        isStopped = _isStopped;
    }
}
