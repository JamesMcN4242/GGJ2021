////////////////////////////////////////////////////////////
/////   TransformUtilities.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////

using UnityEngine;

namespace PersonalFramework
{
    public static class TransformUtilities
    {
        public static void DestroyAllChildren(this Transform parent)
        {
            for (int i = parent.childCount - 1; i >= 0; i--)
            {
                Object.Destroy(parent.GetChild(i).gameObject);
            }
        }
    }
}