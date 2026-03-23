using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

namespace Won
{
    public class UI_Base : MonoBehaviour
    {
        protected Dictionary<Type, UnityEngine.Object[]> _objects = new();

        protected bool _init = false;

        private void Awake()
        {
            Init();
        }

        protected virtual bool Init()
        {
            if (_init) return false;

            return _init = true;
        }

        protected void Bind<T>(Type type) where T : UnityEngine.Object
        {
            string[] names = Enum.GetNames(type);
            UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];

            for (int i = 0; i < objects.Length; i++)
            {
                if (typeof(T) == typeof(GameObject))
                    objects[i] = Utils.FindChild(gameObject, names[i]);
                else
                    objects[i] = Utils.FindChild<T>(gameObject, names[i]);

                if (objects[i] == null)
                    Debug.Log($"Failed to bind({names[i]})");

            }
            _objects.Add(typeof(T), objects);
        }

        public void BindObject(Type type) { Bind<GameObject>(type); }
        public void BindImage(Type type) { Bind<Image>(type); }
        public void BindText(Type type) { Bind<TextMeshProUGUI>(type); }
        public void BindButton(Type type) { Bind<Button>(type); }

        protected T Get<T>(int idx) where T : UnityEngine.Object
        {
            if (_objects.TryGetValue(typeof(T), out UnityEngine.Object[] objects))
            {
                return objects[idx] as T;
            }
            return null;
        }

        public GameObject GetObject(int idx) { return Get<GameObject>(idx); }
        public Image GetImage(int idx) { return Get<Image>(idx); }
        public TextMeshProUGUI GetText(int idx) { return Get<TextMeshProUGUI>(idx); }
        public Button GetButton(int idx) { return Get<Button>(idx); }


    }
}