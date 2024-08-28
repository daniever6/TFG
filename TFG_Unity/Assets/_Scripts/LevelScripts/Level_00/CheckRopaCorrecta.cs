using _Scripts.Utilities;
using Unity.VisualScripting;
using UnityEngine;

namespace _Scripts.LevelScripts.Level_00
{
    /// <summary>
    /// Clase que comprueba si la ropa elegida por el jugador es correcta o no
    /// </summary>
    public class CheckRopaCorrecta : MonoBehaviour
    {
        private ClothingIndex _playerClothes;
        private ClothingIndex _correctClothes = new()
        {
            headIdx = -1,
            shirtIdx = 2,
            gloveIdx = 0,
            pantsIdx = 1,
            shoesIdx = 2
        };

        private void Start()
        {
            string path = Application.persistentDataPath + "/clothingIndex.json";
            if (System.IO.File.Exists(path))
            {
                string json = System.IO.File.ReadAllText(path);
                _playerClothes = JsonUtility.FromJson<ClothingIndex>(json);
            }
        }

        /// <summary>
        /// Comprueba si el personaje lleva la ropa correcta o no
        /// </summary>
        /// <returns>True si es correcta. False si no es correcta</returns>
        public bool CheckClothes()
        {
            if (_playerClothes.IsUnityNull()) return false;
            
            if (_correctClothes.shirtIdx == _playerClothes.shirtIdx &&
                _correctClothes.gloveIdx == _playerClothes.gloveIdx &&
                _correctClothes.pantsIdx == _playerClothes.pantsIdx &&
                _correctClothes.shoesIdx == _playerClothes.shirtIdx)
            {
                return true;
            }
            
            return false;
        }
    }
}
