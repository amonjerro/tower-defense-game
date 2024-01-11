using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceUIManager : MonoBehaviour
{
    public TextMeshProUGUI funds;
    public TextMeshProUGUI score;
    private void Awake()
    {
        ResourceManager.OnUpdateScore += UpdateTexts;
    }

    private void UpdateTexts()
    {
        score.text = ResourceManager.instance.score.ToString();
        funds.text = ResourceManager.instance.money.ToString();
    }

    private void Start()
    {
        UpdateTexts();
    }
}
