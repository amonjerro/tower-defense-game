using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType {
    Wall,
    Floor
};

public class Tile
{
    

    public int X {get; private set;}
    public int Y {get; private set;}
    public TileType Type {get; private set;}

    public Tile (int x, int y, TileType type){
        this.X = x;
        this.Y = y;
        this.Type = type;
    }
}
