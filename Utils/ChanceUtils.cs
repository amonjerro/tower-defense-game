using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChanceUtils
{
    static System.Random _r = new System.Random();
    public static bool Heads(){
        float coin_flip = UnityEngine.Random.Range(0.0f,1.0f);
        return coin_flip > 0.5;
    }

    public static T RandomEnumValue<T>(){
        Array v = Enum.GetValues(typeof(T));
        return (T) v.GetValue(_r.Next(v.Length));
    }
}
