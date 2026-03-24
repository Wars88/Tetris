using UnityEngine;

namespace Won
{
    public class BaseController : MonoBehaviour
    {
        private bool _init = false;

        private void Awake()
        {
            Init();
        }

        private void Update()
        {
            UpdateController();
        }

        public virtual bool Init()
        {
            if (_init)
                return false;

            _init = true;
            return true;
        }
        public virtual void UpdateController(){}
        
    }
}