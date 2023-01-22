using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoordinateVector
{
    public int X { get; private set;}
    public int Y { get; private set;}

    public static float nudgeAmount = 0.5f;
    
    public CoordinateVector(int x, int y){
        this.X = x;
        this.Y = y;
    }

    public int Distance(CoordinateVector other){
        return Math.Abs(this.X - other.X) + Math.Abs(this.Y - other.Y);
    }

    public int Distance_X(CoordinateVector other){
        return Math.Abs(this.X - other.X);
    }

    public int Distance_Y(CoordinateVector other){
        return Math.Abs(this.Y - other.Y);
    }

    public Vector3 ToVector3(bool nudge){
        if (nudge){
            return new Vector3(this.X + nudgeAmount, this.Y + nudgeAmount, 0);
        }
        return new Vector3(this.X, this.Y, 0);
    }

    public bool Equals(CoordinateVector b){
        if (b is null){
            return false;
        }

        return this.X == b.X  && this.Y == b.Y;
    }
    public override bool Equals(object obj) => this.Equals(obj as CoordinateVector);

    public static bool operator == (CoordinateVector a, CoordinateVector b){
        if (a is null){
            if (b is null){
                //null == null
                return true;
            }
            return false;
        }

        return a.Equals(b);
    }

    public static bool operator != (CoordinateVector a, CoordinateVector b){
        if (a is null){
            if (b is null){
                //null == null
                return false;
            }
            return true;
        }

        return !a.Equals(b);
    }

    public override int GetHashCode() => (X, Y).GetHashCode();

    public override string ToString(){
        return "X:"+this.X.ToString()+",Y:"+this.Y.ToString();
    }

    public static CoordinateVector FindMidPoint(int x1, int x2, int y1, int y2){
        int min_x;
        int max_x;

        int min_y;
        int max_y;

        //Find the order
        if (x1 <= x2){
            min_x = x1;
            max_x = x2;
        } else {
            min_x = x2;
            max_x = x1;
        }

        if (y1 <= y2){
            min_y = y1;
            max_y = y2;
        } else {
            min_y = y2;
            max_y = y1;
        }

        //Perform the operation
        int x_out = (max_x - min_x)/2 + min_x;
        int y_out = (max_y - min_y)/2 + min_y;

        return new CoordinateVector(x_out, y_out);
    }

}

public class CorridorCoordinates
{
    public CoordinateVector CoordinateA {get; private set;}
    public CoordinateVector CoordinateB {get; private set;}
    public CorridorCoordinates(CoordinateVector a, CoordinateVector b){
        this.CoordinateA = a;
        this.CoordinateB = b;
    }
}