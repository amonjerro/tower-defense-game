

public class ControlUtils{

    public static void WhileControl(int current_value, int control_check, string message){
        if(current_value >= control_check){
            throw new System.Exception(message);
        }
    }
}