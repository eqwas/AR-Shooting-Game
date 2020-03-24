using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveScene : MonoBehaviour
{
    
    public void goGameScene()
    {
        SceneManager.LoadScene("ARGameProject");
    }
    public void goMainScene()
    {
        SceneManager.LoadScene("Menu");
    }
    public void gameExit()
    {
        Application.Quit();
    }
}
