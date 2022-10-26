using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCoordinateVector : MonoBehaviour
{
    
    void Start() {

        // Initalize a queue with a node
        CoordinateVector midPoint = CoordinateVector.FindMidPoint(3,5,3,1);
        if(!TestUtils.AssertEqual(midPoint.X, 4)) throw new TestUtilsException("Unexpected X value for midpoint: "+midPoint.X.ToString());
        if(!TestUtils.AssertEqual(midPoint.Y, 2)) throw new TestUtilsException("Unexpected X value for midpoint: "+midPoint.Y.ToString());

    }
}
