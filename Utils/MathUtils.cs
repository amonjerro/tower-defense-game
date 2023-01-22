using System;

public class MathUtils {
    public static float NormalizeIntToFloat(int min, int max, int val){
        float min_float = (float) min;
        float max_float = (float) max;
        float val_float = (float) val;
        return (val_float-min_float) / (max_float-min_float);
    }

    public static int RandomOddIntFromRange(int min, int max){
        bool heads = ChanceUtils.Heads();
        int val;
        Random rnd = new Random();
        val = rnd.Next(min, max);

        if (val % 2 == 0){
            val += heads ? 1 : -1;
        }
        return val;
    }
}