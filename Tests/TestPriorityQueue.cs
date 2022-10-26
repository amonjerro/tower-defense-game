using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPriorityQueue : MonoBehaviour
{
    
    void Start() {

        // Initalize a queue with a node
        PriorityQueue queue = new PriorityQueue(new VectorNode(1, new CoordinateVector(1,1)));

        if (!TestUtils.AssertEqual(queue.Peek().sortable_value, 1)) throw new TestUtilsException("Unexpected root value");

        queue.Add(new VectorNode(2, new CoordinateVector(2,1)));
        CoordinateVector returned = queue.PopRoot();

        if(!TestUtils.AssertEqual(returned.X, 1)) throw new TestUtilsException("Returned root has unexpected values");
        if(!TestUtils.AssertEqual(queue.Peek().sortable_value, 2)) throw new TestUtilsException("Remaining root has unexpected value");

        queue.Add(new VectorNode(1, new CoordinateVector(1,1)));
        if (!TestUtils.AssertEqual(queue.Peek().sortable_value, 1)) throw new TestUtilsException("Unexpected root value");

        queue.Clear();

        //Add multiple elements in disorder
        queue.Add(new VectorNode(4, new CoordinateVector(4,1)));
        queue.Add(new VectorNode(1, new CoordinateVector(1,1)));
        queue.Add(new VectorNode(5, new CoordinateVector(5,1)));
        queue.Add(new VectorNode(2, new CoordinateVector(2,1)));


        //Elements should be returned in order
        returned = queue.PopRoot();
        if(!TestUtils.AssertEqual(returned.X, 1)) throw new TestUtilsException("Returned root has unexpected values");

        returned = queue.PopRoot();
        if(!TestUtils.AssertEqual(returned.X, 2)) throw new TestUtilsException("Returned root has unexpected values");

        returned = queue.PopRoot();
        if(!TestUtils.AssertEqual(returned.X, 4)) throw new TestUtilsException("Returned root has unexpected values");

        returned = queue.PopRoot();
        if(!TestUtils.AssertEqual(returned.X, 5)) throw new TestUtilsException("Returned root has unexpected values");
    }   

}
