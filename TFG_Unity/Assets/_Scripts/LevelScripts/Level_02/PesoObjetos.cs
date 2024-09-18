using UnityEngine;

namespace _Scripts.LevelScripts.Level_02
{
    public class PesoObjetos : MonoBehaviour
    {
        public float Peso = 0;
        [SerializeField] private PesoObjetos[] pesosSecundarios;

        /// <summary>
        /// Devuelve el peso total del objeto
        /// </summary>
        /// <returns>Float del peso total</returns>
        public float GetPesoTotal()
        {
            var pesoTotal = Peso;

            foreach (var objeto in pesosSecundarios)
            {
                pesoTotal += objeto.Peso;
            }

            return pesoTotal;
        }
    }
}
