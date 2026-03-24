using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static Won.Define;

namespace Won
{
    public class InputManager
    {
        private InputActionAsset _inputActionAsset;

        // 구독(Subscribe)할 이벤트들
        public Action OnMoveLeft = null;
        public Action OnMoveRight = null;
        public Action OnMoveDown = null;
        public Action OnRotate = null;
        public Action OnHardDrop = null;

        // 키를 뗐을 때 발생하는 이벤트 (DAS Last Input Wins용)
        public Action OnCanceledLeft = null;
        public Action OnCanceledRight = null;

        // 소프트 드롭 상태
        public bool IsSoftDropping { get; private set; }
        // DAS 계산을 위해 현재 눌려있는 방향키 상태 노출
        public bool IsPressingLeft  { get; private set; }
        public bool IsPressingRight { get; private set; }

        public void Init()
        {
            InputActionAsset asset = Managers.ResourceManager.Load<InputActionAsset>(InputActionPath);

            if (asset == null)
            {
                Debug.Log("InputActionAsset not found.");
                return;
            }

            _inputActionAsset = asset;
            var moveLeft  = asset.FindAction(LeftActionPath);
            var moveRight = asset.FindAction(RightActionPath);
            var moveDown  = asset.FindAction(DownActionPath);
            var rotate    = asset.FindAction(RotateActionPath);
            var hardDrop  = asset.FindAction(HardDropActionPath);

            moveLeft.performed  += _ => { IsPressingLeft  = true;  OnMoveLeft?.Invoke(); };
            moveLeft.canceled   += _ => { IsPressingLeft  = false; OnCanceledLeft?.Invoke(); };

            moveRight.performed += _ => { IsPressingRight = true;  OnMoveRight?.Invoke(); };
            moveRight.canceled  += _ => { IsPressingRight = false; OnCanceledRight?.Invoke(); };

            moveDown.performed  += _ => { IsSoftDropping  = true;  OnMoveDown?.Invoke(); };
            moveDown.canceled   += _ =>   IsSoftDropping  = false;

            rotate.performed    += _ => OnRotate?.Invoke();
            hardDrop.performed  += _ => OnHardDrop?.Invoke();

            asset.Enable();
        }

        public void SetActive(bool isActive)
        {
            if (isActive) _inputActionAsset?.Enable();
            else          _inputActionAsset?.Disable();
        }

        public void Clear()
        {
            OnMoveLeft     = null;
            OnMoveRight    = null;
            OnMoveDown     = null;
            OnRotate       = null;
            OnHardDrop     = null;
            OnCanceledLeft = null;
            OnCanceledRight= null;

            _inputActionAsset?.Disable();
            _inputActionAsset = null;
        }
    }
}
