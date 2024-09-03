using UnityEngine;

public static class Vector3Extentions
{
    public static Color32 ToColor32(this Vector3 v)
    {
        return new Color32((byte)v.x, (byte)v.y, (byte)v.z, 255);
    }

    public static Vector3 ToVector3(this Color32 c)
    {
        return new Vector3(c.r, c.g, c.b);
    }
}

