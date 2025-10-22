using UnityEngine;

public class Extras : MonoBehaviour
{
    public static bool NearlyEqual(float a, float b, float epsilon)
    {
        return Mathf.Abs(a - b) < epsilon;
    }
}
