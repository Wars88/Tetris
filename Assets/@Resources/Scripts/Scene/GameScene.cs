
using UnityEngine;
using static Won.Define;

namespace Won
{
    public class GameScene : BaseScene
    {
        protected override bool Init()
        {
            if (base.Init() == false)
                return false;

            Managers.UIManager.ShowSceneUI<UI_GameScene>();
            Managers.ResourceManager.Instantiate(TetrisControllerPath);

            Debug.Log("초기화 완료");
            return true;
        }
    }
}