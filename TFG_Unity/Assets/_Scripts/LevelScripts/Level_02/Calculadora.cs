using System;
using System.Data;
using System.Linq;
using _Scripts.Utilities;
using TMPro;
using UnityEngine;

namespace _Scripts.LevelScripts.Level_02
{
    public class Calculadora : GameplayMonoBehaviour<Calculadora>
    {
        [SerializeField] private GameObject calculadoraCanvas;
        [SerializeField] private TextMeshProUGUI pantalla;

        private char[] Operators = { 'x', '/', '+', '-', '(', ')' };
        private char[] Numbers = { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };
        private bool isMathError = false;

        private void Start()
        {
            calculadoraCanvas.SetActive(false);
        }

        /// <summary>
        /// Introduce un char al texto de la pantalla
        /// </summary>
        /// <param name="button">Char a introducir</param>
        public void EnterNumber(string button)
        {
            if (isMathError)
            {
                SetCero();
                isMathError = false;
            }
            
            pantalla.text += button;
        }

        /// <summary>
        /// Metodo para introducir una coma en la calculadora.
        /// Evita poner varias comas seguidas
        /// </summary>
        public void EnterComma()
        {
            if (isMathError)
            {
                SetCero();
                isMathError = false;
            }
            
            if (pantalla.text.Length <= 0)
            {
                return;
            }
            
            if (Numbers.Contains(pantalla.text[pantalla.text.Length - 1]))
            {
                pantalla.text += ',';
            }
        }

        /// <summary>
        /// Borra el texto de la pantalla
        /// </summary>
        public void SetCero()
        {
            pantalla.text = String.Empty;
        }

        /// <summary>
        /// Elimina el ultimo char de la pantalla
        /// </summary>
        public void DeleteLastChar()
        {
            if (isMathError)
            {
                SetCero();
                isMathError = false;
            }
            
            if (pantalla.text.Length <= 0)
            {
                return;
            }

            pantalla.text = pantalla.text.Substring(0, pantalla.text.Length - 1);
        }

        /// <summary>
        /// Devuelve el resultado del a operacion y lo muestra por pantalla
        /// </summary>
        public void GetResult()
        {
            string operacion = pantalla.text.Replace('x', '*').Replace(',', '.');
            try
            {
                DataTable dataTable = new DataTable();

                var result = dataTable.Compute(operacion, "");

                Double final = Convert.ToDouble(result);

                //Si supera esos limites, hacer el numero exponencial
                if ((Mathf.Abs((int)final) >= 1000000 || Mathf.Abs((int)final) < 0.001f))
                {
                    pantalla.text = final.ToString("E");
                }
                else
                {
                    pantalla.text = final.ToString();
                }

                foreach (var character in final.ToString())
                {
                    if (char.IsLetter(character))
                    {
                        isMathError = true;
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                pantalla.text = "MATH ERROR";
                isMathError = true;
            }
        }

        /// <summary>
        /// Cierra el canvas de la calculadora
        /// </summary>
        public void CloseCalculadora()
        {
            calculadoraCanvas.SetActive(false);
        }

        /// <summary>
        /// Muestra el canvas de la calculadora
        /// </summary>
        public void OpenCalculadora()
        {
            calculadoraCanvas.SetActive(true);
        }

        protected override void OnPostPaused()
        {
            base.OnPostPaused();
            calculadoraCanvas.SetActive(false);
        }
    }
}
