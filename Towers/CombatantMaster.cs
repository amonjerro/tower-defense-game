using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatantMaster : MonoBehaviour
{
    public GameObject Turret;
    public GameObject Body;
    public GameObject death;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TimeToDie(){
        //Play some animation
        ScoreHolder scoreHolder = GetComponent<ScoreHolder>();
        if (scoreHolder != null)
        {
            scoreHolder.Reward();
        }
        Explode();
        
    }


    void Explode()
    {
        Turret.SetActive(false);
        Body.SetActive(false);
        death.SetActive(true);
        Animator anim = death.GetComponent<Animator>();
        anim.SetTrigger("t_death");
    }

    void FadeAway(){
        ScoreHolder scoreHolder = GetComponent<ScoreHolder>();
        if (scoreHolder != null)
        {
            scoreHolder.Punish();
        }
        Destroy(gameObject);
    }
}
