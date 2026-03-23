using UnityEngine;

namespace Won
{
    public static class Extension
    {
        public static T GetOrAddComponent<T>(this GameObject go) where T : Component
        {
            return Utils.GetOrAddComponent<T>(go);
        }
    }
}