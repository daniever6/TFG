using UnityEngine;
using UnityEngine.UI;

namespace Dialogues
{
    [System.Serializable]
    public class Dialogue
    {
        [SerializeField] private Sprite _unitImage;
        [SerializeField] private string _unitName;
        [TextArea(3, 10)] [SerializeField] private string[] _sentences;

        public string[] Sentences => _sentences;
        public string UnitName => _unitName;
        public Sprite UnitImage => _unitImage;

        public static implicit operator Dialogue(DialogueSerializable dialogue)
        {
            return new Dialogue()
            {
                _unitImage = dialogue.UnitImage,
                _unitName = dialogue.UnitName,
                _sentences = new string[]{dialogue.Sentences}
            };
        }
    }
}