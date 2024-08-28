using System;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace _Scripts.LevelScripts.SaveManager
{
    public static class SaveManager
    {
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
