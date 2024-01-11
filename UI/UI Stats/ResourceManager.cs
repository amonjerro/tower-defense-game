using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager instance;
    public int score;
    public int money;

    public delegate void ScoreUpdated();
    public static event ScoreUpdated OnUpdateScore;


    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        } else
        {
            instance = this;
        }
    }

    public void AddToScore(int value)
    {
        Debug.Log("Calling Add To Score");
        score += value;
        money += value / 10;
        if (OnUpdateScore != null)
        {
            OnUpdateScore();
        }
    }

    public void SubtractFromScore(int value)
    {
        if (score - value < 0)
        {
            score = 0;
        } else
        {
            score -= value;
        }

        if (OnUpdateScore != null)
        {
            OnUpdateScore();
        }
        
    }

    public void SpendMoney(int value)
    {
        money -= value;
        if(OnUpdateScore != null)
        {
            OnUpdateScore();
        }
    }
}
