using System;
using System.Diagnostics;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public static Action GameOver;

    [SerializeField] GameObject gameOverScreen;
    [SerializeField] bool haveGameOver = true;

    private void OnEnable()
    {
        GameOver += AtGameOver;
    }

    private void OnDisable()
    {
        GameOver -= AtGameOver;
    }

    void AtGameOver()
    {
        if (!haveGameOver) return;
        Time.timeScale = 0f;
        gameOverScreen.SetActive(true);
    }
}
