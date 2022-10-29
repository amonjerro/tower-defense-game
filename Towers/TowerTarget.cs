using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerTarget : MonoBehaviour
{
    
    public int maxHp;
    private int currentHp;

    // Start is called before the first frame update
    void Start()
    {
        this.currentHp = this.maxHp;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void TakeDamage(int damage){
        this.currentHp -= damage;
        if (this.currentHp <= 0){
            // Expand on this
            SendMessageUpwards("TimeToDie");
        }
    }
}
