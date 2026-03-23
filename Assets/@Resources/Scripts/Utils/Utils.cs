using UnityEngine;

namespace Won
{
    public class Utils
    {
        public static T FindChild<T>(GameObject parent, string name = null) where T : UnityEngine.Object
        {
            if (parent == null)
                return null;

            foreach (var component in parent.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                {
                    return component;
                }
            }

            return null;
        }

        public static GameObject FindChild(GameObject parent, string name = null)
        {
            Transform transform = FindChild<Transform>(parent, name);
            if (transform == null)
                return null;

            return transform.gameObject;
        }

        public static T GetOrAddComponent<T>(GameObject go) where T : Component
        {
            T component = go.GetComponent<T>();
            if (component == null)
                component = go.AddComponent<T>();
            return component;
        }
    }
}