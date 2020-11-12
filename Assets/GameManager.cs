using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public WaveSpawner waveSpawner;
    public UserInterfaceManager uiManager;

    public static GameManager instance;

    void Start()
    {
        instance = this;

        if (uiManager != null)
            uiManager.ScreenStart();
    }
    public void Lose()
    {
        waveSpawner.ResetSpawner();
        uiManager.ScreenLose();
    }
    public void Win()
    {
        uiManager.ScreenWin();
    }
    public void LevelStart() 
    {
        waveSpawner.StartLevel();
    }
    public static void Pause(bool pause)
    {
        if (pause)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }
}
