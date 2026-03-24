using UnityEngine;

namespace Won
{
    
    public class Block : MonoBehaviour
    {
        private enum SpriteRenderers
        {
            BlockBg
        }

        private SpriteRenderer _spriteRend;

        public void Init(Color color)
        {
            if (_spriteRend == null)
            {
                _spriteRend = Utils.FindChild<SpriteRenderer>(
                    gameObject,SpriteRenderers.BlockBg.ToString());
            }
            if (_spriteRend == null)
            {
                Debug.Log("SpriteRenderer not found.");
                return;
            }

            _spriteRend.color = color;
        }
    }
}