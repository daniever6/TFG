using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace _Scripts.LevelScripts.Level_02
{
    public class BalanzaManager : Utilities.Singleton<BalanzaManager>
    {
        [SerializeField] private TextMeshProUGUI textoPantalla;
        [SerializeField] private GameObject BalanzaPesoPadre;
        
        public static bool IsBalanzaOn;
        public static bool IsBalanzaReady;
        public static bool IsBalanzaOpen = false;
        
        private static float Peso = 0;
        private const string UNITS = "g";
        private const string FORMAT = "F4";

        private void Start()
        {
            textoPantalla.text = string.Empty;
        }

        /// <summary>
        /// Establece el peso de la balanza
        /// </summary>
        /// <param name="newValue">Peso nuevo de la balanza</param>
        public void SetPesoBalanza(float newValue)
        {
            Peso = newValue;
            
            Math.Clamp(Peso, 0, Double.PositiveInfinity);
            
            if(IsBalanzaReady)  SetBalanzaText($"{Peso.ToString(FORMAT)} {UNITS}");
        }

        /// <summary>
        /// Suma un valor al peso de la balanza, si es negativo lo resta
        /// </summary>
        /// <param name="value">Valor a acumular</param>
        public void AddSubPeso(float value)
        {
            Peso += value;
            
            Math.Clamp(Peso, 0, Double.PositiveInfinity);
            
            if(IsBalanzaReady) SetBalanzaText($"{Peso.ToString(FORMAT)} {UNITS}");
        }

        /// <summary>
        /// Establece el texto de la pantalla de la balanza
        /// </summary>
        /// <param name="newText">Texto a mostrar</param>
        public void SetBalanzaText(string newText)
        {
            textoPantalla.text = newText;
        }

        /// <summary>
        /// Calcula el peso actual de la balanza y lo muestra por la pantalla
        /// </summary>
        /// <returns>Peso actual de la balanza</returns>
        public float CalculateCurrentPesos()
        {
            if (BalanzaPesoPadre.transform.childCount <= 0) return 0;
            
            var objetoInBalanza = BalanzaPesoPadre.transform.GetChild(0).gameObject;
            
            Peso = objetoInBalanza.GetComponent<PesoObjetos>().GetPesoTotal();
            
            Math.Clamp(Peso, 0, Double.PositiveInfinity);
            
            SetPesoBalanza(Peso);
            
            return Peso;
        }
    }
}