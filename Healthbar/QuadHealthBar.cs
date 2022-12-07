using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadHealthBar : MonoBehaviour
{

    public float health;
    public float healthDisplay;
    public float healthChangeSpeed;
    private MeshRenderer meshRenderer;


    // Start is called before the first frame update
    void Start()
    {
        healthDisplay = health;
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health < healthDisplay){
            healthDisplay -= healthChangeSpeed;
        }
        meshRenderer.material.SetFloat("_Health", healthDisplay);
    }

    public void SetHealth(float value){
        health = value;
    }
}
