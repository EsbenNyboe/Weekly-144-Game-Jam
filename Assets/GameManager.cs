using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public WaveSpawner waveSpawner;
    public UserInterfaceManager uiManager;

    void Start()
    {
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
        waveSpawner.SpawnWave();
    }
    public static void Pause(bool pause)
    {
        if (pause)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }
}
