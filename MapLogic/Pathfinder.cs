using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder
{

    private static Pathfinder _instance = null;
    private bool path_created = false;

    private Pathfinder(){}

    private static readonly object _lock = new object();


    //Make Singleton
    public static Pathfinder GetInstance(){
        if (_instance == null){
            lock(_lock){
                if (_instance == null){
                    _instance = new Pathfinder();
                    _instance.path = new List<CoordinateVector>();
                }
            }
        }
        return _instance;
    }

    private List<CoordinateVector> path;

    public int PathSize(){
        return _instance.path.Count;
    }

    private int Heuristic(CoordinateVector x, CoordinateVector end){
        int x_diff = end.X-x.X;
        int y_diff = end.Y-x.Y;

        return (int)Math.Round(Math.Sqrt(x_diff*x_diff+y_diff*y_diff));
    }

    private void ReconstructPath(Dictionary<string, CoordinateVector> cameFrom, CoordinateVector current){
        _instance.path.Add(current);
        while(cameFrom.ContainsKey(current.ToString())){
            current = cameFrom[current.ToString()];
            _instance.path.Add(current);
        }
        _instance.path.Reverse();
        _instance.path_created = true;
    }

    public bool IsPathCreated(){
        return _instance.path_created;
    }

    public CoordinateVector GetNext(int current){
        if (current+1 >= _instance.path.Count){
            return null;
        }
        return _instance.path[current+1];
    }

    public bool FindPath(MapGenerator map){
        CoordinateVector start = map.GetStartingPosition();
        CoordinateVector end = map.GetEndingPosition();

        int tentative_gScore;
        int neighbor_gScore;

        PriorityQueue pQueue = new PriorityQueue(new VectorNode(0, start));
        CoordinateVector current;

        Dictionary<string, CoordinateVector> cameFrom = new Dictionary<string, CoordinateVector>();
        Dictionary<string, int> gScore = new Dictionary<string, int>();
        gScore.Add(start.ToString(), 0);

        Dictionary<string, int> fScore = new Dictionary<string, int>();
        fScore.Add(start.ToString(), Heuristic(start, end));

        while(pQueue.count > 0){
            current = pQueue.PopRoot();

            if (current == end){
                _instance.ReconstructPath(cameFrom, current);
                return true;
            }

            foreach(var neighbor in map.GetNeighbors(current, 1)){
                tentative_gScore = gScore[current.ToString()] + current.Distance(neighbor);
                neighbor_gScore = gScore.TryGetValue(neighbor.ToString(), out int value) ? value : int.MaxValue;
                if (tentative_gScore < neighbor_gScore){
                    cameFrom[neighbor.ToString()] = current;
                    if (gScore.ContainsKey(neighbor.ToString())){
                        gScore[neighbor.ToString()] = tentative_gScore;
                    } else {
                        gScore.Add(neighbor.ToString(), tentative_gScore);
                    }
                    
                    if (fScore.ContainsKey(neighbor.ToString())){
                        fScore[neighbor.ToString()] = tentative_gScore + Heuristic(neighbor, end);
                    } else {
                        fScore.Add(neighbor.ToString(), tentative_gScore + Heuristic(neighbor, end));
                    }
                    
                    if (!pQueue.Find(neighbor)){

                        pQueue.Add(new VectorNode(tentative_gScore + Heuristic(neighbor, end), neighbor));
                    }
                }
            }

        }
        Debug.Log("Could not find end");
        return false;

    }



}
