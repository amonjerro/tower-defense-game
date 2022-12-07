
public class MathUtils {
    public static float NormalizeIntToFloat(int min, int max, int val){
        float min_float = (float) min;
        float max_float = (float) max;
        float val_float = (float) val;
        return (val_float-min_float) / (max_float-min_float);
    }
}