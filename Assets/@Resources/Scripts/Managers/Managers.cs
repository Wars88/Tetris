using UnityEngine;
using static Won.Define;

namespace Won
{
    public class Managers : MonoBehaviour
    {
        private static Managers s_instance;
        public static Managers Instance { get { Init(); return s_instance; }}

        private ResourceManager _rescourceManager = new ResourceManager();
        private SoundManager _soundManager = new SoundManager();
        private UIManager _uiManager = new UIManager();
        private InputManager _inputManager = new InputManager(); // 🆕 추가

        public static ResourceManager ResourceManager { get { return Instance?._rescourceManager; }}
        public static SoundManager SoundManager { get { return Instance?._soundManager; }}
        public static UIManager UIManager { get { return Instance?._uiManager; }}
        public static InputManager InputManager { get { return Instance?._inputManager; }} // 🆕 추가

        public static void Init()
        {
            if (s_instance == null)
            {
                GameObject go = GameObject.Find(ManagersPath);
            
                if (go == null)
                {
                    go = new GameObject(ManagersPath);
                    go.AddComponent<Managers>();
                }
                DontDestroyOnLoad(go);
                s_instance = go.GetComponent<Managers>();

                s_instance._inputManager.Init();
            }
        }

    }
}