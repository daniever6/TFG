using UnityEngine;
using UnityEngine.UI;

namespace Dialogues
{
    [System.Serializable]
    public class Dialogue
    {
        public string UnitName;
        public Sprite UnitImage;
        [TextArea(3, 10)] public string[] Sentences;
    }
}