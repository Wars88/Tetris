namespace Won
{
    public class UI_Popup : UI_Base
    {

        protected override bool Init()
        {
            if (base.Init() == false)
                return false;
        
            Managers.UIManager.SetCanvas(gameObject, true);

            return true;
        }
    }
}