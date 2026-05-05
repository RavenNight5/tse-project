using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameReset : MonoBehaviour
{
    //resets the game by reloading the scene
    public void Reset()
    {
        SceneManager.SetActiveScene(SceneManager.GetActiveScene());
    }

    //quits the game application
    public void Exit()
    {
        Application.Quit();
    }
}
