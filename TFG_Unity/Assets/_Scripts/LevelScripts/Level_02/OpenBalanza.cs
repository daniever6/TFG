using System;
using Unity.VisualScripting;
using UnityEngine;

namespace _Scripts.LevelScripts.Level_02
{
    public class OpenBalanza : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        private void OnMouseUp()
        {
            if (!BalanzaManager.IsBalanzaOpen)
            {
                _animator.Play("OpenBalanza");
                BalanzaManager.IsBalanzaOpen = true;
            }
            else
            {
                _animator.Play("CloseBalanza");
                BalanzaManager.IsBalanzaOpen = false;
            }
        }
    }
}
