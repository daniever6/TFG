using System;
using _Scripts.Utilities;
using UnityEngine;

namespace _Scripts.LevelScripts.Level_02
{
    public class ReactivoPesaje : Singleton<ReactivoPesaje>
    {
        [SerializeField] private GameObject reactivo;
        private PesoObjetos pesoReactivo;
        private Vector3 initialScale;

        /// <summary>
        /// Devuelve la cantidad de reactivo actual
        /// </summary>
        /// <returns></returns>
        public float CurrentPesoReactivo()
        {
            return pesoReactivo.Peso;
        }

        private void Start()
        {
            pesoReactivo = reactivo.GetComponent<PesoObjetos>();
            initialScale = reactivo.transform.localScale;
            
            pesoReactivo.Peso = 0;
            reactivo.transform.localScale = Vector3.zero;
            reactivo.gameObject.SetActive(false);
        }

        /// <summary>
        /// Añade una cantidad de reactivo al papel de pesaje
        /// </summary>
        /// <param name="peso">Cantidad a añadir</param>
        public void AddReactivo(float peso)
        {
            pesoReactivo.Peso += peso;
            reactivo.transform.localScale += initialScale/2;
            
            BalanzaManager.Instance.AddSubPeso(peso);

            OnPostPesoAdded();
        }

        /// <summary>
        /// Quita algo de peso al reactivo
        /// </summary>
        /// <param name="peso">Cantidad a quitar</param>
        public void RemoveReactivo(float peso)
        {
            pesoReactivo.Peso += peso;
            reactivo.transform.localScale -= initialScale/2;
            
            BalanzaManager.Instance.AddSubPeso(peso);
            
            OnPostPesoAdded();
        }

        /// <summary>
        /// Elimina todo el reactivo del papel de pesaje
        /// </summary>
        public void SetReactivoCero()
        {
            pesoReactivo.Peso = 0;
            
            OnPostPesoAdded();
        }

        /// <summary>
        /// Se ejecuta despues de modificar el peso del reactivo. Calculara su escala y visibilidad
        /// </summary>
        private void OnPostPesoAdded()
        {
            reactivo.gameObject.SetActive(pesoReactivo.Peso > 0);
            
            //Calcular nueva escala del reactivo
            Vector3 newScale = Vector3.zero;
            newScale.z = Clamp(reactivo.transform.localScale.z, initialScale.z, 23.54454f);
            if (newScale.z <= 12.36518f)
            {
                newScale.x = newScale.y = Clamp(reactivo.transform.localScale.x, initialScale.x, 1.161928f);
            }
            else
            {
                newScale.x = newScale.y = 1.161928f;
            }

            reactivo.transform.localScale = newScale;
            
            if (pesoReactivo.Peso <= 0)
            {
                reactivo.transform.localScale = Vector3.zero;
            }
        }

        /// <summary>
        /// Devuelve un valor entre un minimo y maximo
        /// </summary>
        /// <param name="value">Valor a calcular</param>
        /// <param name="min">Minimo</param>
        /// <param name="max">Maximo</param>
        /// <returns>Valor entre [min, max]</returns>
        private float Clamp(float value, float min, float max)
        {
            return value < min ? min :
                value > max ? max : value;
        }
    }
}
