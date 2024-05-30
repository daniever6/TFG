namespace _Scripts.Utilities
{
    public enum LevelState
    {
        None,
        Tutorial,
        FirstLevel,
        SecondLevel,
        ThirdLevel
    }
    
    public enum GameState
    {
        Starting,
        Resume,
        Dialogue,
        Pause,
        Win,
        Loose
    }
    
    public enum CombinationResult {
        None,
        Error,
        Correct,
        Explosion,
        Corrosion
    }
    
    public enum Iteractables
    {
        None,
        Ground,
        Alfombrilla,
        Npc,
        Interactable,
    }
    
    public enum PlayerState
    {
        FirstPerson = 0,
        ThirdPerson = 1
    }
}