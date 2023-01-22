using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float spawnRate;
    public float tickLimit;
    private float currentSpawnCount;

    public GameObject DefaultTankPrefab;
    public GameObject DoubleCannonTank;

    private Pathfinder pathfinder;

    public Queue<EnemyTypes> enemies;


    // Start is called before the first frame update
    void Start()
    {
        this.currentSpawnCount = 0.0f;
        this.enemies = new Queue<EnemyTypes>();

        for (int i = 0; i < 2; i++){
            enemies.Enqueue(ChanceUtils.RandomEnumValue<EnemyTypes>());
        }
    }

    public void SetupPathfinder(MapGenerator map, int entrance){
        this.pathfinder = new Pathfinder(entrance);
        this.pathfinder.FindPath(map);
    }

    // Update is called once per frame
    void Update()
    {
        if (enemies.Count > 0 && pathfinder.IsPathCreated()){
            //Do the whole spawn evaluation
            this.EvaluateEnemySpawn();    
        }   
    }

    private void SpawnEnemy(EnemyTypes type){
        GameObject go;
        if (type == EnemyTypes.Tank){
            go = Instantiate(DefaultTankPrefab, transform.position, transform.rotation);
            go = go.transform.GetChild(1).gameObject;
            Enemy enemy = go.GetComponent<Enemy>();
            enemy.SetPathfinder(this.pathfinder);
        } else if(type == EnemyTypes.DualCannonTank){
            go = Instantiate(DoubleCannonTank, transform.position, transform.rotation);
            go = go.transform.GetChild(1).gameObject;
            Enemy enemy = go.GetComponent<Enemy>();
            enemy.SetPathfinder(this.pathfinder);
        }
    }

    private void EvaluateEnemySpawn(){
        this.currentSpawnCount += Time.deltaTime * this.spawnRate;

        if (this.currentSpawnCount >= this.tickLimit){
            // Reset counter
            this.currentSpawnCount = 0.0f;
            // Create a new enemy
            this.SpawnEnemy(enemies.Dequeue());
        }
    }
}
