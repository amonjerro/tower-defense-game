using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder
{

    private bool path_created = false;
    private int _entrance;

    public Pathfinder(int entrance){
        this.path = new List<CoordinateVector>();
        this._entrance = entrance;
    }

    private List<CoordinateVector> path;

    public int PathSize(){
        return path.Count;
    }

    private int Heuristic(CoordinateVector x, CoordinateVector end){
        int x_diff = end.X-x.X;
        int y_diff = end.Y-x.Y;

        return (int)Math.Round(Math.Sqrt(x_diff*x_diff+y_diff*y_diff));
    }

    private void ReconstructPath(Dictionary<string, CoordinateVector> cameFrom, CoordinateVector current){
       path.Add(current);
        while(cameFrom.ContainsKey(current.ToString())){
            current = cameFrom[current.ToString()];
           path.Add(current);
        }
       path.Reverse();
       path_created = true;
    }

    public bool IsPathCreated(){
        return path_created;
    }

    public CoordinateVector GetNext(int current){
        if (current+1 >=path.Count){
            return null;
        }
        return path[current+1];
    }

    public bool FindPath(MapGenerator map){
        CoordinateVector start = map.GetEntrance(this._entrance);
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
               ReconstructPath(cameFrom, current);
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
