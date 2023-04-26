using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    DeathHandler deathHandler;

    void Awake()
    {
        deathHandler = FindObjectOfType<DeathHandler>();
    }

    public void ReloadGame()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        deathHandler.SetIsStopped(false);
    }
}
