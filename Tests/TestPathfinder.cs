using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPathfinder : MonoBehaviour
{
    private bool pathFinder_tested = false;
    private Pathfinder pathfinder;

    void Start() {

        this.pathfinder = new Pathfinder(0);

        if (pathfinder.IsPathCreated()){
            if(!TestUtils.AssertGreaterThan(pathfinder.PathSize(), 0)) throw new TestUtilsException("No Path Found");
            pathFinder_tested = true;
        }
    }

    void Update(){
        if (!pathFinder_tested){
            if (pathfinder.IsPathCreated()){
                if(!TestUtils.AssertGreaterThan(pathfinder.PathSize(), 0)) throw new TestUtilsException("No Path Found");
                pathFinder_tested = true;
            }
        }
    }
}
