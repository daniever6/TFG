using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.LevelScripts.Level_00
{
    public enum BodyPart
    {
        Hair,
        Shirt,
        Glove,
        Pants,
        Shoes
    }
    
    [Serializable]
    public class ClothItemContainer
    {
        public BodyPart key;
        public List<Image> collection;
    }
    
    [System.Serializable]
    public class ClothingIndex
    {
        public int headIdx = 2;
        public int shirtIdx = -1;
        public int gloveIdx = -1;
        public int pantsIdx = -1;
        public int shoesIdx = 0;
    }
    
    public class ClothSelector : MonoBehaviour
    {
        [SerializeField] private List<ClothItemContainer> clothButtons;
        
        [SerializeField] private List<GameObject> hairComponents;
        [SerializeField] private List<GameObject> shirtComponents;
        [SerializeField] private List<GameObject> gloveComponents;
        [SerializeField] private List<GameObject> pantsComponents;
        [SerializeField] private List<GameObject> shoesComponents;
        
        private Color _desactivateColor = new Color(0.6f, 0.6f,0.6f, 1);
        private Color _outlineColor = Color.black;
        
        private ClothingIndex _clothingIndex = new ();

        private void Start()
        {
            UISelectorRopa.OnSaveClothChanges += SaveClothingIndex;
            LoadClothingIndex();
            
            ActivateComponent(hairComponents, _clothingIndex.headIdx);
            ActivateComponent(shirtComponents, _clothingIndex.shirtIdx);
            ActivateComponent(gloveComponents, _clothingIndex.gloveIdx);
            ActivateComponent(pantsComponents, _clothingIndex.pantsIdx);
            ActivateComponent(shoesComponents, _clothingIndex.shoesIdx);

            foreach (var itemContainer in clothButtons)
            {
                switch (itemContainer.key)
                {
                    case BodyPart.Hair:
                        SetItemColor(itemContainer.collection, _clothingIndex.headIdx + 1 );
                        break;
                    
                    case BodyPart.Shirt:
                        SetItemColor(itemContainer.collection, _clothingIndex.shirtIdx + 1 );
                        break;
                    
                    case BodyPart.Glove:
                        SetItemColor(itemContainer.collection, _clothingIndex.gloveIdx + 1 );
                        break;
                    
                    case BodyPart.Pants:
                        SetItemColor(itemContainer.collection, _clothingIndex.pantsIdx + 1);
                        break;
                    
                    case BodyPart.Shoes:
                        SetItemColor(itemContainer.collection, _clothingIndex.shoesIdx);
                        break;
                }
            }
        }
        
        /// <summary>
        /// Guarda la ropa que ha seleccionado en un Json llamado clothingIndex.json
        /// </summary>
        public void SaveClothingIndex()
        {
            string json = JsonUtility.ToJson(_clothingIndex);
            System.IO.File.WriteAllText(Application.persistentDataPath + "/clothingIndex.json", json);
        }

        /// <summary>
        /// Carga los indices de la ropa seleccionada del json en caso de que exista
        /// </summary>
        /// <returns>True si carga los datos</returns>
        public bool LoadClothingIndex()
        {
            string path = Application.persistentDataPath + "/clothingIndex.json";
            if (System.IO.File.Exists(path))
            {
                string json = System.IO.File.ReadAllText(path);
                _clothingIndex = JsonUtility.FromJson<ClothingIndex>(json);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Establece el color por defecto de los botones de la interfaz. Si no esta seleccionado, sera gris,
        /// si esta seleccionado sera blanco
        /// </summary>
        /// <param name="collection">Lista de botones de ropa</param>
        /// <param name="currentIdx">Boton activado</param>
        private void SetItemColor(List<Image> collection, int currentIdx)
        {
            int cont = 0;
            foreach (var button in collection)
            {
                Outline outline = button.gameObject.AddComponent<Outline>();
                outline.effectColor = _outlineColor;
                outline.effectDistance = new Vector2(4, 4);

                if (cont != currentIdx)
                {
                    outline.enabled = false;
                }
                
                button.color = (cont == currentIdx) ? Color.white : _desactivateColor;
                cont++;
            }
        }

        /// <summary>
        /// Activa los componentes(Ropa) seleccionados y los otros los desactiva
        /// </summary>
        /// <param name="components">Lista de componentes</param>
        /// <param name="activeIdx">Indice seleccionado</param>
        private void ActivateComponent(List<GameObject> components, int activeIdx)
        {
            for (int i = 0; i < components.Count; i++)
            {
                components[i].SetActive(i == activeIdx);
            }
        }

        /// <summary>
        /// Selecciona el nuevo item de ropa seleccionado
        /// </summary>
        /// <param name="item">String formado por "ParteCuerpo_IndiceSeleccionado"</param>
        public void SelectCloth(string item)
        {
            BodyPart bodyPart = (BodyPart) Enum.Parse(typeof(BodyPart), item.Split("_")[0]);
            int idxSelected = Convert.ToInt32(item.Split("_")[1]);

            switch (bodyPart)
            {
                case BodyPart.Hair:
                    ItemSelectionColor(_clothingIndex.headIdx + 1, idxSelected + 1, BodyPart.Hair);
                    SelectionLogic(ref _clothingIndex.headIdx, idxSelected, hairComponents);
                    break;
                case BodyPart.Shirt:
                    ItemSelectionColor(_clothingIndex.shirtIdx + 1, idxSelected + 1, BodyPart.Shirt);
                    SelectionLogic(ref _clothingIndex.shirtIdx, idxSelected, shirtComponents);
                    break;
                case BodyPart.Glove:
                    ItemSelectionColor(_clothingIndex.gloveIdx + 1, idxSelected + 1, BodyPart.Glove);
                    SelectionLogic(ref _clothingIndex.gloveIdx, idxSelected, gloveComponents);
                    break;
                case BodyPart.Pants:
                    ItemSelectionColor(_clothingIndex.pantsIdx + 1, idxSelected + 1, BodyPart.Pants);
                    SelectionLogic(ref _clothingIndex.pantsIdx, idxSelected, pantsComponents);
                    break;
                case BodyPart.Shoes:
                    ItemSelectionColor(_clothingIndex.shoesIdx, idxSelected, BodyPart.Shoes);
                    SelectionLogic(ref _clothingIndex.shoesIdx, idxSelected, shoesComponents);
                    break;
            }
        }

        /// <summary>
        /// Desactiva el item anterior y selecciona el item nuevo
        /// </summary>
        /// <param name="currentIdx">Indice del item a desactivar</param>
        /// <param name="newIdx">Indice del nuevo item</param>
        /// <param name="items">Lista de los componentes de la ropa</param>
        private void SelectionLogic(ref int currentIdx, int newIdx, List<GameObject> items)
        {
            if(currentIdx >= 0) items[currentIdx].SetActive(false);
            currentIdx = newIdx;
            
            if (newIdx < 0)
            {
                return;
            }
            
            items[newIdx].SetActive(true);
        }

        /// <summary>
        /// Cambia el color del boton del item seleccionado
        /// </summary>
        /// <param name="currentIdx">Item anterior</param>
        /// <param name="newIdx">Item seleccionado</param>
        /// <param name="bodyPart">Parte del cuerpo</param>
        public void ItemSelectionColor(int currentIdx ,int newIdx, BodyPart bodyPart)
        {
            var buttons = clothButtons[(int)bodyPart].collection;

            buttons[currentIdx].color = _desactivateColor;
            buttons[currentIdx].gameObject.GetComponent<Outline>().enabled = false;
            
            buttons[newIdx].color = Color.white;
            buttons[newIdx].gameObject.GetComponent<Outline>().enabled = true;
        }

    }
}
