using UnityEngine;
using UnityEngine.UI;

namespace Dialogues
{
    [System.Serializable]
    public class Dialogue
    {
        [SerializeField] private Image _unitImage;
        [SerializeField] private string _unitName;
        [TextArea(3, 10)] [SerializeField] private string[] _sentences;

        public string[] Sentences => _sentences;
        public string UnitName => _unitName;
        public Image UnitImage => UnitImage;
    }
}