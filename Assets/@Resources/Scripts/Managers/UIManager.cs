using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Won.Define;

namespace Won
{
    public class UIManager
    {
        private int _order = 10;
        private Stack<UI_Popup> _popupStack = new();
        private UI_Scene _sceneUI = null;
        public UI_Scene SceneUI { get { return _sceneUI; }}

        public GameObject Root
        {
            get
            {
                GameObject root = GameObject.Find(UIRootPath);
                if (root == null)
                    root = new GameObject(UIRootPath);
                
                return root;
            }
        }

        public void SetCanvas(GameObject go, bool isSort = true, int sortOrder = 0)
        {
            Canvas canvas = go.GetOrAddComponent<Canvas>();

            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.overrideSorting = true;

            CanvasScaler scaler = go.GetOrAddComponent<CanvasScaler>();
            if (scaler != null)
            {
                scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                scaler.referenceResolution = new Vector2(1080, 1920);
            }
            go.GetOrAddComponent<GraphicRaycaster>();
            
            if (isSort)
            {
                canvas.sortingOrder = _order;
                _order++;
            }
            else
                canvas.sortingOrder = sortOrder;
        }

        public T ShowSceneUI<T>() where T : UI_Scene
        {
            GameObject go = Managers.ResourceManager.Instantiate(typeof(T).Name);
            if (go == null)
            {
                Debug.LogWarning($"{typeof(T).Name} prefab not found.");
                return null;
            }

            T sceneUI = go.GetOrAddComponent<T>();
            _sceneUI = sceneUI;
            
            go.transform.SetParent(Root.transform);
            return sceneUI;
        }

        public T ShowPopupUI<T>() where T : UI_Popup
        {
            GameObject go = Managers.ResourceManager.Load<GameObject>(typeof(T).Name);
            T popupUI = go.GetOrAddComponent<T>();
            _popupStack.Push(popupUI);
            
            go.transform.SetParent(Root.transform, false);

            return popupUI;
        }

        public void ClostPopupUI()
        {
            if (_popupStack.Count == 0)
                return;
            
            var popup = _popupStack.Pop();
            _order--;
            Managers.ResourceManager.Destroy(popup.gameObject);
        }

    }
}