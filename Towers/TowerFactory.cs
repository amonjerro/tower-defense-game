using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TowerFactory : MonoBehaviour 
{
    public GameObject basicTower;

    public GameObject Make(TowerTypes type, Vector3 where){
        if (type == TowerTypes.TestTower){
            return this.CreateTestTower(where);
        }
        
        return null;
    }

    private GameObject CreateTestTower(Vector3 where){
        return Instantiate(
            basicTower,
            where, Quaternion.identity
        );
    }
}
