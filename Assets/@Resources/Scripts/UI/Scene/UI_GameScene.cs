namespace Won
{
    public class UI_GameScene : UI_Scene
    {
        private enum Images
        {
            NextBlockImage1,
            NextBlockImage2,
            NextBlockImage3
        }

        private enum Texts
        {
            ScoreText,
            LevelText,
            LineScoreText
        }

        private enum Buttons
        {
            PosButton
        }

        protected override bool Init()
        {
            if (base.Init() == false)
                return false;

            BindImage(typeof(Images));
            BindText(typeof(Texts));
            BindButton(typeof(Buttons));

            
            RefreshUI();
            return true;
        }

        public void SetInfo()
        {
            
            RefreshUI();
        }

        private void RefreshUI()
        {
            
        }
    }
}