using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInterfaceManager : MonoBehaviour
{
    public GameObject textTitle;
    public GameObject textLose;
    public GameObject textWin;

    public GameObject buttonStart;
    public GameObject buttonRestart;
    public GameObject buttonUnpause;
    public GameObject buttonExit;

    bool inMenu;
    bool inGame;
    float timer;

    public GameManager gameManager;

    void Awake()
    {
        textTitle.SetActive(false);
        textLose.SetActive(false);
        textWin.SetActive(false);
        buttonStart.SetActive(false);
        buttonRestart.SetActive(false);
        buttonUnpause.SetActive(false);
        buttonExit.SetActive(false);
    }
    void Update()
    {
        timer++;
        if (Input.GetKeyDown(KeyCode.Escape) && inGame)
        {
            if (timer > 1)
            {
                timer = 0;
                if (!inMenu)
                    PauseGame();
                else
                    UnpauseGame();
            }
        }

        if (Input.GetKeyDown(KeyCode.W))
            ScreenWin();
        if (Input.GetKeyDown(KeyCode.L))
            ScreenLose();
    }
    public void ScreenStart()
    {
        textTitle.SetActive(true);
        buttonStart.SetActive(true);
        StaticCamera();
    }
    public void ScreenLose() // triggered from ui
    {
        textLose.SetActive(true);
        DisplayUI();
        StaticCamera();
        inGame = false;
    }
    public void ScreenWin() // triggered from ui
    {
        print("win");
        textWin.SetActive(true);
        DisplayUI();
        StaticCamera();
        inGame = false;
    }
    public void LevelStart() // triggered from ui
    {
        gameManager.LevelStart();
        MovingCamera();
        inGame = true;
    }
    public void DisplayUI()
    {
        buttonRestart.SetActive(true);
        buttonExit.SetActive(true);
    }

    public void ExitGame() // triggered from ui
    {
        Application.Quit();
    }
    public void PauseGame() // triggered from ui
    {
        inMenu = true;
        textTitle.SetActive(true);
        DisplayUI();
        buttonUnpause.SetActive(true);
        GameManager.Pause(true);
        StaticCamera();
    }
    public void UnpauseGame() // triggered from ui
    {
        inMenu = false;
        textTitle.SetActive(false);
        textLose.SetActive(false);
        textWin.SetActive(false);
        buttonRestart.SetActive(false);
        buttonUnpause.SetActive(false);
        buttonExit.SetActive(false);
        GameManager.Pause(false);
        MovingCamera();
    }


    void StaticCamera()
    {
        //HEEEELP: how to switch between static and moving camera
    }
    void MovingCamera()
    {
        //HELPP
    }
}
