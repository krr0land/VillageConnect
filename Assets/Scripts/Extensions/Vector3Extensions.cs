using UnityEngine;

public class Vector3Extensions
{
    public static Vector3 WithXZ(Vector2 input)
    {
        return new Vector3(input.x, 0, input.y);
    }
}