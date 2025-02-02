using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerTarget : MonoBehaviour
{
    
    public int maxHp;
    private int currentHp;
    public QuadHealthBar healthBar;

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
        healthBar.SetHealth(MathUtils.NormalizeIntToFloat(0, this.maxHp, this.currentHp));
        if (this.currentHp <= 0){
            // Expand on this
            SendMessageUpwards("TimeToDie");
        }
    }
    
    private void OnTriggerExit2D(Collider2D other){
        if(other.gameObject.tag == "Enemy"){
            other.gameObject.SendMessage("ReleaseTarget");
        }
    }
}
