using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    // Start is called before the first frame update

    public Slider slider;

    public void SetHealth(int health){
        slider.value = health;
    }

    public void SetMax(int max){
        slider.maxValue = max;
        slider.value = max;
    }
}
