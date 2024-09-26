using System;
using Unity.VisualScripting;
using UnityEngine;

namespace _Scripts.LevelScripts.Level_02
{
    public class OpenCalculadora : MonoBehaviour
    {
        private void OnMouseUp()
        {
            Calculadora.Instance.OpenCalculadora();
        }
    }
}
