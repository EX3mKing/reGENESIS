using UnityEngine;

public class Extras : MonoBehaviour
{
    public static bool NearlyEqual(float a, float b, float epsilon)
    {
        return Mathf.Abs(a - b) < epsilon;
    }
    
    public static bool NearlyEqual(Vector3 a, Vector3 b, float epsilon)
    {
        return Vector3.Distance(a, b) < epsilon;
    }
}
