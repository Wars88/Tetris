using UnityEngine;
using static Won.Define;

namespace Won
{
    public class Managers : MonoBehaviour
    {
        private static Managers s_instance;
        public static Managers Instance { get { Init(); return s_instance; }}

        private BoardManager _boardManager = new BoardManager();
        private ResourceManager _rescourceManager = new ResourceManager();
        private SoundManager _soundManager = new SoundManager();
        private UIManager _uiManager = new UIManager();

        

        public static BoardManager BoardManager { get { return Instance?._boardManager; }}
        public static ResourceManager ResourceManager { get { return Instance?._rescourceManager; }}
        public static SoundManager SoundManager { get { return Instance?._soundManager; }}
        public static UIManager UIManager { get { return Instance?._uiManager; }}
        

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
            }
        }

    }
}