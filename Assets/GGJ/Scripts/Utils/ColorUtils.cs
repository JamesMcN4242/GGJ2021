using UnityEngine;

public static class ColorUtils
{
    public static Color CopyWithR(this Color col, float val)
    {
        return new Color(val, col.g, col.b, col.a);
    }
    
    public static Color CopyWithG(this Color col, float val)
    {
        return new Color(col.r, val, col.b, col.a);
    }
    
    public static Color CopyWithB(this Color col, float val)
    {
        return new Color(col.r, col.g, val, col.a);
    }

    public static Color CopyWithA(this Color col, float val)
    {
        return new Color(col.r, col.g, col.b, val);
    }
}