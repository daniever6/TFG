using System.Collections.Generic;
using Dialogues;
using UnityEngine;

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
public class DialogueSerializable
{
    public string UnitName;
    public Sprite UnitImage;
    public string Sentences;
}

