using System.Collections;
using System.Collections.Generic;

public class TestUtils
{
    public static bool AssertEqual(int a, int b){
        return a == b;
    }

    public static bool AssertEqual(float a, float b){
        return a == b;
    }

    public static bool AssertGreaterThan(int a, int b){
        return a > b;
    }

    public static bool AssertGreaterThan(float a, float b){
        return a > b;
    }

    public static bool AssertGreaterOrEqualTo(int a, int b){
        return a >= b;
    }

    public static bool AssertGreaterOrEqualTo(float a, float b){
        return a >= b;
    }

}


[System.Serializable]
public class TestUtilsException : System.Exception
{
    public TestUtilsException() { }
    public TestUtilsException(string message) : base(message) { }
    public TestUtilsException(string message, System.Exception inner) : base(message, inner) { }
    protected TestUtilsException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}