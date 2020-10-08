using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPoint : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        GameManager.SI.SetGameState(GameState.win);
    }
}

