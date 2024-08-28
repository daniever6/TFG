using _Scripts.Managers;
using _Scripts.Utilities;
using UnityEngine;

namespace _Scripts.LevelScripts.SaveManager
{
    [System.Serializable]
    public class SaveData
    {
        public float[] playerPosition = {0f, 0f, 0f};
        public GameState gameState;
        public LevelState levelState;

        public SaveData(GameObject player)
        {
            var playerPos = player.transform.position;
            playerPosition[0] = playerPos.x;
            playerPosition[1] = playerPos.y;
            playerPosition[2] = playerPos.z;
            gameState = GameManager.GameState;
            levelState = LevelManager.Instance.CurrentLevelState;
            
        }
    }
}
