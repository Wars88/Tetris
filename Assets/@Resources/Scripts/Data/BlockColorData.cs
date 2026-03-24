using UnityEngine;
using static Won.Define;

namespace Won
{
    [CreateAssetMenu(fileName = "New Color Data", menuName = "Block Color Data", order = 1)]
    public class BlockColorData : ScriptableObject
    {
        [SerializeField] private Color[] _colors = new Color[7];

        public Color GetColor(Tetromino type)
        {
            return _colors[(int)type];
        }
    }
}