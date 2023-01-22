using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Flags]
public enum Directions {
    North = 1 << 0,
    East = 1 << 2,
    South = 1 << 3,
    West = 1 << 1,
}


public class MapGenerator
{

    public readonly int rows;
    public readonly int columns;

    public List<CoordinateVector> entrances;
    public List<CoordinateVector> starting_corridors;

    public int ending_x;
    public int ending_y;

    public TileType[][] map;

    private List<CoordinateVector> DeadEnds;
    
    public MapGenerator(int x, int y){
        this.rows = y;
        this.map = new TileType[this.rows][];
        this.columns = x;
        this.DeadEnds = new List<CoordinateVector>();
        this.entrances = new List<CoordinateVector>();
        this.starting_corridors = new List<CoordinateVector>();
        for(int i = 0; i < y; i++){
            this.map[i] = new TileType[this.columns];
            for(int k = 0; k < x; k++){
                this.map[i][k] = TileType.Wall;
            }
        }
    }

    private void UpdateCoordinateValue(int x, int y, TileType new_value){
        this.map[y][x] = new_value;
    }

    public CoordinateVector GetEntrance(int index){
        return this.entrances[index];
    }

    public CoordinateVector GetEndingPosition(){
        return new CoordinateVector(this.ending_x, this.ending_y);
    }

    public void CreateEntrance(){
        int y_wall, x_wall;
        int starting_corridor_x, starting_corridor_y;
        starting_corridor_x = starting_corridor_y = 0;

        bool y_fixed;
        int a = MathUtils.RandomOddIntFromRange(1,(this.columns-2)/2);
        int b = MathUtils.RandomOddIntFromRange(1,((this.rows -2)/2));
        
        //Pick a random wall point
        x_wall = UnityEngine.Random.Range(0.0f,1.0f) > 0.5f ? 0 : 1;
        y_wall = UnityEngine.Random.Range(0.0f,1.0f) > 0.5f ? 0 : 1;
        y_fixed = UnityEngine.Random.Range(0.0f, 1.0f) > 0.5f;

        if (y_fixed){
            starting_corridor_y = ((this.rows-3)*y_wall)+1;
            x_wall = x_wall * (this.columns-1-2*a) + a;
            y_wall = y_wall * (this.rows-1);
            starting_corridor_x = x_wall;
            
        } else {
            starting_corridor_x = ((this.columns-3)*x_wall)+1;
            y_wall = y_wall * (this.rows-1-2*b) + b;
            x_wall = x_wall * (this.columns-1);
            starting_corridor_y = y_wall;
        }

        this.entrances.Add(new CoordinateVector(x_wall, y_wall));
        this.starting_corridors.Add(new CoordinateVector(starting_corridor_x, starting_corridor_y));
        this.UpdateCoordinateValue(starting_corridor_x, starting_corridor_y, TileType.Floor);
        this.UpdateCoordinateValue(x_wall, y_wall, TileType.Floor);

    }

    private void CreateExit(){

        //Establish a minimum distance
        int min_distance = 2;
        int max_retries = 10;
        int retries = 0;
        int a = MathUtils.RandomOddIntFromRange(1,(this.columns-2)/2);
        int b = MathUtils.RandomOddIntFromRange(1,((this.rows -2)/2));
        bool is_okay = false;
        int x_wall = 0;
        int y_wall = 0;
        bool y_fixed = false;

        //Randomly probe the walls until you find a proper exit
        while(!is_okay){
            ControlUtils.WhileControl(retries, max_retries, "Exceeded retries creating exit");
            //Pick a random wall point
            x_wall = UnityEngine.Random.Range(0.0f,1.0f) > 0.5f ? 0 : 1;
            y_wall = UnityEngine.Random.Range(0.0f,1.0f) > 0.5f ? 0 : 1;
            y_fixed = UnityEngine.Random.Range(0.0f, 1.0f) > 0.5f;

            if (y_fixed){
                x_wall = (x_wall * (this.columns-1-2*a)) + a;
                y_wall = (y_wall * (this.rows-1));
            } else {
                y_wall = (y_wall * (this.rows-1-2*b)) + b;
                x_wall = x_wall * (this.columns-1);
            }
            
            CoordinateVector possible_exit = new CoordinateVector(x_wall, y_wall);

            is_okay = true;
            for (int i = 0; i < this.entrances.Count; i++){
                if (possible_exit.Distance_X(this.entrances[i]) <= min_distance && possible_exit.Distance_Y(this.entrances[i]) <= min_distance){
                    is_okay = false;
                    break;
                }
            }
            retries++;
        }

        this.ending_x = x_wall;
        this.ending_y = y_wall;

        this.UpdateCoordinateValue(this.ending_x, this.ending_y, TileType.Floor);
    }

    public bool IsWall(int x, int y){
        return this.map[y][x] == TileType.Wall;
    }

    public bool IsEnd(int x, int y){
        return x == this.ending_x && y == this.ending_y;
    }

    private bool IsTileType(int x, int y, TileType value){
        return this.map[y][x] == value;
    }

    private void ExpandHorizon(CoordinateVector location, int min_x, int max_x, int min_y, int max_y, List<CoordinateVector> horizon){
        int loc_x = location.X;
        int loc_y = location.Y;
        int horizons_expanded = 0;

        // Check north
        if ((loc_y-2 >= min_y || this.IsEnd(loc_x, loc_y-2)) && (this.IsWall(loc_x, loc_y-2) || this.IsEnd(loc_x, loc_y-2))){
            this.UpdateCoordinateValue(loc_x, loc_y-1, TileType.Floor);
            this.UpdateCoordinateValue(loc_x, loc_y-2, TileType.Floor);
            horizon.Add(new CoordinateVector(loc_x, loc_y-2));
            horizons_expanded++;
        }

        // Check East
        if ((loc_x -2 >= min_x || this.IsEnd(loc_x-2, loc_y)) && (this.IsWall(loc_x-2, loc_y) || this.IsEnd(loc_x-2, loc_y))){
            this.UpdateCoordinateValue(loc_x-1, loc_y, TileType.Floor);
            this.UpdateCoordinateValue(loc_x-2, loc_y, TileType.Floor);
            horizon.Add(new CoordinateVector(loc_x - 2, loc_y));
            horizons_expanded++;
        }

        //Check West
        if ((loc_x + 2 <= max_x || this.IsEnd(loc_x+2, loc_y)) && (this.IsWall(loc_x+2, loc_y) || this.IsEnd(loc_x+2, loc_y))){
            this.UpdateCoordinateValue(loc_x+1, loc_y, TileType.Floor);
            this.UpdateCoordinateValue(loc_x+2, loc_y, TileType.Floor);
            horizon.Add(new CoordinateVector(loc_x + 2, loc_y));
            horizons_expanded++;
        }

        //Check South
        if ((loc_y + 2 <= max_y || this.IsEnd(loc_x, loc_y+2)) && (this.IsWall(loc_x, loc_y+2) || this.IsEnd(loc_x, loc_y+2))){
            this.UpdateCoordinateValue(loc_x, loc_y+1, TileType.Floor);
            this.UpdateCoordinateValue(loc_x, loc_y+2, TileType.Floor);
            horizon.Add(new CoordinateVector(loc_x, loc_y+2));
            horizons_expanded++;
        }

        if(horizons_expanded == 0){
            this.DeadEnds.Add(location);
        }
    }

    private CoordinateVector TestNeighbors(CoordinateVector location, int min_x, int max_x, int min_y, int max_y, TileType testValue){
        int loc_x = location.X;
        int loc_y = location.Y;
        // Check north
        if (loc_y-2 >= min_y && this.IsTileType(loc_x, loc_y-2, testValue)){
            return new CoordinateVector(loc_x, loc_y-1);
        }

        // Check East
        if (loc_x -2 >= min_x && this.IsTileType(loc_x-2, loc_y, testValue)){
            return new CoordinateVector(loc_x-1, loc_y);
        }

        //Check West
        if (loc_x + 2 <= max_x && this.IsTileType(loc_x+2, loc_y, testValue)){
            return new CoordinateVector(loc_x+1, loc_y);
        }

        //Check South
        if (loc_y + 2 <= max_y && this.IsTileType(loc_x, loc_y+2, testValue)){
            return new CoordinateVector(loc_x, loc_y+1);
        }

        return new CoordinateVector(-1, -1);

    } 

    public List<CoordinateVector> GetNeighbors(CoordinateVector location, int distance){
        List<CoordinateVector> neighbors = new List<CoordinateVector>();

        if (location.X >= distance && this.map[location.Y][location.X-distance] != TileType.Wall){
            neighbors.Add(new CoordinateVector(location.X-distance, location.Y));
        }

        if (location.Y >= distance && this.map[location.Y-distance][location.X] != TileType.Wall){
            neighbors.Add(new CoordinateVector(location.X, location.Y-distance));
        }

        if (location.X < this.columns-distance && this.map[location.Y][location.X+distance] != TileType.Wall){
            neighbors.Add(new CoordinateVector(location.X+distance, location.Y));
        }

        if (location.Y < this.rows-distance && this.map[location.Y+distance][location.X] != TileType.Wall){
            neighbors.Add(new CoordinateVector(location.X, location.Y+distance));
        }

        return neighbors;
    }

    private CoordinateVector PluckFromHorizon(List<CoordinateVector> horizon){
        
        CoordinateVector returnable;

        if (ChanceUtils.Heads()){
            //Pop from end
            returnable = horizon[horizon.Count-1];
            horizon.RemoveAt(horizon.Count-1);
            return returnable;
        }

        returnable = horizon[0];
        horizon.RemoveAt(0);
        return returnable;
    }

    public int GetBitMask(int y, int x){
        bool north = y == rows - 1 || (this.map[y+1][x] == TileType.Wall);
        bool east = x == columns - 1 || (this.map[y][x+1] == TileType.Wall);
        bool south = y == 0 || (this.map[y-1][x] == TileType.Wall);
        bool west = x == 0 || (this.map[y][x-1] == TileType.Wall);

        var bitMask = (north ? Directions.North : 0) | (east ? Directions.East : 0) | (south ? Directions.South : 0) | (west ? Directions.West : 0); 
        return (int) bitMask;
    }

    public void ConstructMap(int entrances){
        int max_x = this.columns - 2;
        int max_y = this.rows - 2;
        int min_x, min_y;
        min_x = min_y = 1;

        for (int i = 0; i < entrances; i++){
            CreateEntrance();
        }
        CreateExit();

        CoordinateVector current = this.starting_corridors[0];
        List<CoordinateVector> horizon = new List<CoordinateVector>();

        this.ExpandHorizon(current, min_x, max_x, min_y, max_y, horizon);

        while (horizon.Count > 0){
            current = this.PluckFromHorizon(horizon);

            if (this.IsEnd(current.X, current.Y)){
                continue;
            }
            
            this.ExpandHorizon(current, min_x, max_x, min_y, max_y, horizon);
        }
        
        for (int i = 1; i < this.entrances.Count; i++){
            int retries = 0;
            horizon.Clear();
            current = this.starting_corridors[i];
            CoordinateVector v = this.TestNeighbors(current, min_x, max_x, min_y, max_y, TileType.Floor);
            if (v.X == -1){
                this.ExpandHorizon(current, min_x, max_x, min_y, max_y, horizon);
                while (horizon.Count > 0){
                    ControlUtils.WhileControl(retries, 3, "Exceeded retries on digging new entrance");
                    current = this.PluckFromHorizon(horizon);
                    v = this.TestNeighbors(current, min_x, max_x, min_y, max_y, TileType.Floor);
                    if (v.X != -1){
                        break;
                    }
                    this.ExpandHorizon(current, min_x, max_x, min_y, max_y, horizon);
                    retries++;
                }
            }
            
            UpdateCoordinateValue(v.X, v.Y, TileType.Floor);
        }

        horizon.Clear();

        //Tighten up dead ends
        for (int i = 0; i < this.DeadEnds.Count; i++){
            current = this.DeadEnds[i];
            horizon = this.GetNeighbors(current, 2);
            for (int j = 0; j < horizon.Count; j++){
                if (!this.IsWall(horizon[j].X, horizon[j].Y)){
                    CoordinateVector midPoint = CoordinateVector.FindMidPoint(horizon[j].X, current.X, horizon[j].Y, current.Y);
                    UpdateCoordinateValue(midPoint.X, midPoint.Y, TileType.Floor);
                    break;
                }
            }
            horizon.Clear();
        }
    }

}
