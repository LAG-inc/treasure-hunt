using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class OnWin : UnityEvent
{
}

public enum GameState
{
    inGame,
    pause,
    gameOver,
    win
}

public class GameManager : MonoBehaviour
{
    private Enemy[] _enemiesInScene;

    public static GameManager SI;
    public GameState currentGameState = GameState.inGame;
    public UnityEvent GameOver;

    public OnWin onWin;

    void Awake()
    {
        SI = SI == null ? this : SI;
    }

    private void Start()
    {
        _enemiesInScene = FindObjectsOfType<Enemy>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetGameState(GameState.pause);
        }

        if (currentGameState != GameState.gameOver) return;

        PlayerController.SI.RestartValues();
        foreach (var enemy in _enemiesInScene)
        {
            enemy.Rebind();
        }

        SetGameState(GameState.inGame);
    }

    public void SetGameState(GameState newGameState)
    {
        if (newGameState == GameState.pause)

        {
            //Show pause panel
            UIManager.sharedInstance.ShowPauseMenu();
        }
        else if (newGameState == GameState.gameOver)
        {
            //Play game over timeLine
            GameOver.Invoke();
        }
        else if (newGameState == GameState.win)
        {
            onWin.Invoke();
        }

        this.currentGameState = newGameState;
    }
}
