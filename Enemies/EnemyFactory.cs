using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyFactory
{
    public Queue<EnemyTypes> enemies;


    public EnemyFactory(EnemyTypes[] enemies){
        this.enemies = new Queue<EnemyTypes>();

        for (int e = 0; e < enemies.Length; e++){
            this.enemies.Enqueue(enemies[e]);
        }
    }

    public void Make(EnemyTypes type, Vector3 position){

    }
}

