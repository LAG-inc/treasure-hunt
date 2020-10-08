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
    gameOver
}

public class GameManager : MonoBehaviour
{
    public static GameManager SI;
    public GameState currentGameState = GameState.inGame;
    public UnityEvent GameOver;

    public OnWin onWin;

    void Awake()
    {
        SI = SI == null ? this : SI;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetGameState(GameState.pause);
        }
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

        this.currentGameState = newGameState;
    }
}
