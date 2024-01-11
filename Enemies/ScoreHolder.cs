using UnityEngine;

public class ScoreHolder : MonoBehaviour
{
    public int score;

    public void Reward()
    {
        ResourceManager.instance.AddToScore(score);
    }

    public void Punish()
    {
        ResourceManager.instance.SubtractFromScore(score);
    }
}
