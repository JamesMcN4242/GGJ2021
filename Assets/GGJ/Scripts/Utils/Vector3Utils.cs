using UnityEngine;

public static class Vector3Utils
{
    public static Vector3 CopyWithX(this Vector3 vec, float val)
    {
        return new Vector3(val, vec.y, vec.z);
    }
    
    public static Vector3 CopyWithY(this Vector3 vec, float val)
    {
        return new Vector3(vec.x, val, vec.z);
    }
    
    public static Vector3 CopyWithXZ(this Vector3 vec, float val)
    {
        return new Vector3(vec.x, vec.y, val);
    }
}
