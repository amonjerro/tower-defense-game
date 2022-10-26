using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator
{

    public readonly int rows;
    public readonly int columns;

    public int starting_x;
    public int starting_y;

    public int ending_x;
    public int ending_y;

    public TileType[][] map;

    private List<CoordinateVector> DeadEnds;
    
    public MapGenerator(int x, int y){
        this.rows = y;
        this.map = new TileType[this.rows][];
        this.columns = x;
        this.DeadEnds = new List<CoordinateVector>();
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

    public CoordinateVector GetStartingPosition(){
        return new CoordinateVector(starting_x, starting_y);
    }

    public CoordinateVector GetEndingPosition(){
        return new CoordinateVector(ending_x, ending_y);
    }

    public CoordinateVector CreateEntranceAndExit(){
        float location = Random.Range(0.0f, 1.0f);
        int first_corridor_x = 0;
        int first_corridor_y = 0;
        
        bool is_bottom = location < 0.25;
        if (is_bottom){
            this.starting_y = this.rows-1;
            this.starting_x = Random.Range(1, this.columns-2);

            bool start_even = this.starting_x % 2 == 0;

            first_corridor_x = this.starting_x;
            first_corridor_y = this.starting_y - 1;

            this.ending_y = 0;
            this.ending_x = Random.Range(1, this.columns-2);

            bool end_even = this.ending_x % 2 == 0;
            while(end_even != start_even || this.ending_x == this.starting_x){
                this.ending_x = Random.Range(1, this.columns-2);
                end_even = this.ending_x % 2 == 0;
            }
        }


        bool is_left = location >= 0.25 && location < 0.5;
        if (is_left){
            this.starting_y = Random.Range(1, this.rows-2);
            this.starting_x = 0;

            bool start_even = this.starting_y % 2 == 0;

            first_corridor_x = 1;
            first_corridor_y = this.starting_y;


            this.ending_y = Random.Range(1, this.rows-2);
            this.ending_x = this.columns - 1;

            bool end_even = this.ending_y % 2 == 0;
            while(end_even != start_even || this.ending_y == this.starting_y){
                this.ending_y = Random.Range(1, this.rows-2);
                end_even = this.ending_y % 2 == 0;
            }
        }

        bool is_right = location >= 0.5 && location < 0.75;
        if (is_right){
            this.starting_x = this.columns-1;
            this.starting_y = Random.Range(1, this.rows-2);

            bool start_even = this.starting_y % 2 == 0;

            first_corridor_x = this.starting_x-1;
            first_corridor_y = this.starting_y;

            this.ending_x = 0;
            this.ending_y = Random.Range(1, this.rows-2);

            bool end_even = this.ending_y % 2 == 0;
            while(end_even != start_even || this.ending_y == this.starting_y){
                this.ending_y = Random.Range(1, this.rows-2);
                end_even = this.ending_y % 2 == 0;
            }
        }

        if (!is_right && !is_left && !is_bottom){
            this.starting_x = Random.Range(1, this.columns-2);
            this.starting_y = 0;
            
            bool start_even = this.starting_x % 2 == 0;

            first_corridor_x = this.starting_x;
            first_corridor_y = 1;

            this.ending_x = Random.Range(1, this.columns-2);
            this.ending_y = this.rows - 1;

            bool end_even = this.ending_x % 2 == 0;
            while(end_even != start_even || this.ending_x == this.starting_x){
                this.ending_x = Random.Range(1, this.columns-2);
                end_even = this.ending_x % 2 == 0;
            }
        }

        this.UpdateCoordinateValue(this.starting_x, this.starting_y, TileType.Floor);
        this.UpdateCoordinateValue(this.ending_x, this.ending_y, TileType.Floor);

        return new CoordinateVector(first_corridor_x, first_corridor_y);
    }

    public bool IsWall(int x, int y){
        return this.map[y][x] == TileType.Wall;
    }

    public bool IsEnd(int x, int y){
        return x == this.ending_x && y == this.ending_y;
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

    public void ConstructMap(CoordinateVector start){
        int max_x = this.columns - 2;
        int max_y = this.rows - 2;
        int min_x, min_y;
        min_x = min_y = 1;

        CoordinateVector current = start;

        List<CoordinateVector> horizon = new List<CoordinateVector>();

        this.ExpandHorizon(current, min_x, max_x, min_y, max_y, horizon);

        while (horizon.Count > 0){
            current = this.PluckFromHorizon(horizon);


            if (this.IsEnd(current.X, current.Y)){
                continue;
            }
            
            this.ExpandHorizon(current, min_x, max_x, min_y, max_y, horizon);
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
