using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatantMaster : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetPosition(Vector3 position){
        gameObject.transform.position = position;
    }

    void TimeToDie(){
        //Play some animation
        Destroy(gameObject);
    }

    void FadeAway(){
        Destroy(gameObject);
    }
}
