using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Won
{
    public class ResourceManager
    {
        private Dictionary<string, Object> _resources = new();

        public T Load<T>(string path) where T : Object
        {
            if (_resources.TryGetValue(path, out Object obj))
                return obj as T;

            var loaded = Resources.Load(path);
            if (loaded != null)
                _resources.Add(path, loaded);

            return loaded as T;
        }

        public GameObject Instantiate(string path, Transform parent = null)
        {
            GameObject prefab = Load<GameObject>($"Prefabs/{path}");
            if (prefab == null)
            {
                Debug.Log($"Failed to load prefab : {path}");
                return null;
            }

            GameObject prefabIns = Object.Instantiate(prefab, parent);
            prefabIns.name = path;
            return prefabIns;
        } 

        public void Destroy(GameObject go)
        {
            if (go == null)
                return;

            Object.Destroy(go);
        }
    }
}