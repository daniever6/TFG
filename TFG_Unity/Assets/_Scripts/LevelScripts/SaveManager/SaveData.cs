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

        public bool[] residuosTirados = { false, false, false };

        public SaveData(GameObject player)
        {
            var playerPos = player.transform.position;
            playerPosition[0] = playerPos.x;
            playerPosition[1] = playerPos.y;
            playerPosition[2] = playerPos.z;

            gameState = GameManager.GameState;
            levelState = LevelManager.Instance.CurrentLevelState;

            // Residuos del nivel 3
            residuosTirados[0] = ResiduosDroppedManager.ResiduosDropped[0];
            residuosTirados[1] = ResiduosDroppedManager.ResiduosDropped[1];
            residuosTirados[2] = ResiduosDroppedManager.ResiduosDropped[2];
        }

        public SaveData(float[] position, GameState _gameState, LevelState _levelState) 
        {
            playerPosition[0] = position[0];
            playerPosition[1] = position[1];
            playerPosition[2] = position[2];

            gameState = _gameState;
            levelState = _levelState;

            // Residuos del nivel 3
            residuosTirados[0] = ResiduosDroppedManager.ResiduosDropped[0];
            residuosTirados[1] = ResiduosDroppedManager.ResiduosDropped[1];
            residuosTirados[2] = ResiduosDroppedManager.ResiduosDropped[2];
        }

        public SaveData(bool[] residuos)
        {
            var _saveData = SaveManager.LoadGameData();

            if(_saveData == null)
            {
                return;
            }

            playerPosition[0] = _saveData.playerPosition[0];
            playerPosition[1] = _saveData.playerPosition[1];
            playerPosition[2] = _saveData.playerPosition[2];

            gameState = GameState.Resume;
            levelState = _saveData.levelState;

            // Residuos del nivel 3
            residuosTirados[0] = residuos[0];
            residuosTirados[1] = residuos[1];
            residuosTirados[2] = residuos[2];
        }
    }
}
