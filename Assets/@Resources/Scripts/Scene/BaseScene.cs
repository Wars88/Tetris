using UnityEngine;
using UnityEngine.EventSystems;
using static Won.Define;

namespace Won
{
    public class BaseScene : MonoBehaviour
    {
        private void Awake()
        {
            Init();
        }

        protected virtual bool Init()
        {
            Object obj = GameObject.FindAnyObjectByType(typeof(EventSystem));
            if (obj == null)
                Managers.ResourceManager.Instantiate(EventSystemPath);

            return true;
        }
    }
}