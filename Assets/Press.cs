using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Press : MonoBehaviour
{
	private Animator anim;
    public PauseMenu pauseMenu;
    void Start()
    {
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        
    }
    public void startAnimStart()
    {
        anim.SetTrigger("Active");
    }
    public void startAnimTutorial()
    {
        anim.SetTrigger("ActiveT");
    }
    public void startAnimBack()
    {
        anim.SetTrigger("ActiveB");
    }

    public void startAnimMainMenu()
    {
        anim.SetTrigger("ActiveM");
    }
    public void startAnimExitGame()
    {
        anim.SetTrigger("ActiveE");
    }
    public void startAnimExitGamePauseMenu()
    {
        anim.SetTrigger("ActiveEP");
    }
    public void startAnimMainMenuPauseMenu()
    {
        anim.SetTrigger("ActiveMP");
    }
    public void startAnimResume()
    {
        anim.SetTrigger("ActiveR");
    }
    public void StartGameScene()
    {
        SceneManager.LoadScene("1");
    }

    public void StartTutorialScene()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void BackScene()
    {
        SceneManager.LoadScene("FirstScene");
    }

    public void MainMenuScene()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
