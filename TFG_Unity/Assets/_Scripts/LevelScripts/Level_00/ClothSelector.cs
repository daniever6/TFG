using System;
using System.Collections.Generic;
using UnityEngine;

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
    public class ClothSelector : MonoBehaviour
    {
        [SerializeField] private List<GameObject> hairComponents;
        [SerializeField] private List<GameObject> shirtComponents;
        [SerializeField] private List<GameObject> gloveComponents;
        [SerializeField] private List<GameObject> pantsComponents;
        [SerializeField] private List<GameObject> shoesComponents;

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
        }

        /// <summary>
        /// Activa los componentes seleccionados y los otros los desactiva
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
                    SelectionLogic(ref currentHairIdx, idxSelected, hairComponents);
                    break;
                case BodyPart.Shirt:
                    SelectionLogic(ref currentShirtIdx, idxSelected, shirtComponents);
                    break;
                case BodyPart.Glove:
                    SelectionLogic(ref currentGloveIdx, idxSelected, gloveComponents);
                    break;
                case BodyPart.Pants:
                    SelectionLogic(ref currentPantsIdx, idxSelected, pantsComponents);
                    break;
                case BodyPart.Shoes:
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
    }
}
