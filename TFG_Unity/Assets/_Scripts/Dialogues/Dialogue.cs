using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dialogues
{
    [System.Serializable]
    public class DialogueStages
    {
        public List<DialogueStage> Stages;
    }

    [System.Serializable]
    public class DialogueStage
    {
        public string Stage;
        public List<Dialogue> Dialogues;
    }
    
    [System.Serializable]
    public class Dialogue
    {
        public string UnitName;
        public Sprite UnitImage;
        [TextArea(3, 10)] public string[] Sentences;
    }
}