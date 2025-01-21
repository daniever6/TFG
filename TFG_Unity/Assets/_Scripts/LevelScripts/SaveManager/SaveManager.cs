using System;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using _Scripts.Utilities;

namespace _Scripts.LevelScripts.SaveManager
{
    public static class SaveManager
    {
        /// <summary>
        /// Guarda los datos del juego especificando la posicion del jugador
        /// </summary>
        /// <param name="player">Referencia al gameobject del jugador</param>
        public static void SaveGameData(GameObject player)
        {
            SaveData saveData = new SaveData(player);
            string dataPath = Application.persistentDataPath + "/gameState.save";
            using (FileStream fileStream = new FileStream(dataPath, FileMode.Create))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(fileStream, saveData);
                fileStream.Close();
            }
            
        }

        /// <summary>
        /// Guarda los datos del juego especificando la pos del jugador, el 
        /// estado del juego, y el nivel
        /// </summary>
        /// <param name="playerPos">Posicion del jugador</param>
        /// <param name="gameState">Estado del juego</param>
        /// <param name="levelState">Nivel del juego</param>
        public static void SaveGameData(float[] playerPos, GameState gameState, LevelState levelState)
        {
            SaveData saveData = new SaveData(playerPos, gameState, levelState);
            string dataPath = Application.persistentDataPath + "/gameState.save";
            using (FileStream fileStream = new FileStream(dataPath, FileMode.Create))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(fileStream, saveData);
                fileStream.Close();
            }
            
        }

        /// <summary>
        /// Guarda los residuos que han sido desechados en los contenedores manteniendo
        /// los demas valores guardados
        /// </summary>
        public static void SaveResiduosData()
        {
            SaveData saveData = new SaveData(ResiduosDroppedManager.ResiduosDropped);
            string dataPath = Application.persistentDataPath + "/gameState.save";
            using (FileStream fileStream = new FileStream(dataPath, FileMode.Create))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(fileStream, saveData);
                fileStream.Close();
            }
        }

        public static SaveData LoadGameData()
        {
            string datapath = Application.persistentDataPath + "/gameState.save";
            
            if (!File.Exists(datapath)) return null;
            
            using (FileStream fileStream = new FileStream(datapath, FileMode.Open))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                SaveData saveData = (SaveData)binaryFormatter.Deserialize(fileStream);
                fileStream.Close();
                return saveData;
            };
        }
    }
}
