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
    
    public class ClothSelector : MonoBehaviour
    {
        [SerializeField] private List<ClothItemContainer> clothButtons;
        
        [SerializeField] private List<GameObject> hairComponents;
        [SerializeField] private List<GameObject> shirtComponents;
        [SerializeField] private List<GameObject> gloveComponents;
        [SerializeField] private List<GameObject> pantsComponents;
        [SerializeField] private List<GameObject> shoesComponents;
        
        private Color desactivateColor = new Color(0.6f, 0.6f,0.6f, 1);
        private Color outlineColor = Color.black;

        private int currentHairIdx = 2;
        private int currentShirtIdx = -1;
        private int currentGloveIdx = -1;
        private int currentPantsIdx = -1;
        private int currentShoesIdx = 0;

        private void Start()
        {
            ActivateComponent(hairComponents, currentHairIdx);
            ActivateComponent(shirtComponents, currentShirtIdx);
            ActivateComponent(gloveComponents, currentGloveIdx);
            ActivateComponent(pantsComponents, currentPantsIdx);
            ActivateComponent(shoesComponents, currentShoesIdx);

            foreach (var itemContainer in clothButtons)
            {
                switch (itemContainer.key)
                {
                    case BodyPart.Hair:
                        SetItemColor(itemContainer.collection, currentHairIdx + 1 );
                        break;
                    
                    case BodyPart.Shirt:
                        SetItemColor(itemContainer.collection, currentShirtIdx + 1 );
                        break;
                    
                    case BodyPart.Glove:
                        SetItemColor(itemContainer.collection, currentGloveIdx + 1 );
                        break;
                    
                    case BodyPart.Pants:
                        SetItemColor(itemContainer.collection, currentPantsIdx + 1);
                        break;
                    
                    case BodyPart.Shoes:
                        SetItemColor(itemContainer.collection, currentShoesIdx);
                        break;
                }
            }
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
                outline.effectColor = outlineColor;
                outline.effectDistance = new Vector2(4, 4);

                if (cont != currentIdx)
                {
                    outline.enabled = false;
                }
                
                button.color = (cont == currentIdx) ? Color.white : desactivateColor;
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
                    ItemSelectionColor(currentHairIdx + 1, idxSelected + 1, BodyPart.Hair);
                    SelectionLogic(ref currentHairIdx, idxSelected, hairComponents);
                    break;
                case BodyPart.Shirt:
                    ItemSelectionColor(currentShirtIdx + 1, idxSelected + 1, BodyPart.Shirt);
                    SelectionLogic(ref currentShirtIdx, idxSelected, shirtComponents);
                    break;
                case BodyPart.Glove:
                    ItemSelectionColor(currentGloveIdx + 1, idxSelected + 1, BodyPart.Glove);
                    SelectionLogic(ref currentGloveIdx, idxSelected, gloveComponents);
                    break;
                case BodyPart.Pants:
                    ItemSelectionColor(currentPantsIdx + 1, idxSelected + 1, BodyPart.Pants);
                    SelectionLogic(ref currentPantsIdx, idxSelected, pantsComponents);
                    break;
                case BodyPart.Shoes:
                    ItemSelectionColor(currentShoesIdx, idxSelected, BodyPart.Shoes);
                    SelectionLogic(ref currentShoesIdx, idxSelected, shoesComponents);
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

            buttons[currentIdx].color = desactivateColor;
            buttons[currentIdx].gameObject.GetComponent<Outline>().enabled = false;
            
            buttons[newIdx].color = Color.white;
            buttons[newIdx].gameObject.GetComponent<Outline>().enabled = true;
        }

    }
}
