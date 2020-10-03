using System;
using UnityEngine;
using UnityEngine.Events;


[Serializable]
public class OnWin : UnityEvent
{
}

public class GameManager : MonoBehaviour
{
    public static GameManager SI;

    [SerializeField]
    private int coinsInLevel;
    [SerializeField]
    private int collectedCoins = 0;

    public OnWin onWin;

    void Awake()
    {
        SI = SI == null ? this : SI;
        coinsInLevel = GameObject.FindObjectsOfType(typeof(Coin)).Length;
    }

    void Update()
    {
        if (collectedCoins >= coinsInLevel)
        {
            Debug.Log("GANASTE WEY!");
            onWin.Invoke();
        }
    }

    public void CollectCoin()
    {
        collectedCoins++;
    }
}
