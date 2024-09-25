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
                OpenBalanzaAnimation();
            }
            else
            {
                CloseBalanzaAnimation();
            }
        }

        public void OpenBalanzaAnimation()
        {
            _animator.Play("OpenBalanza");
            BalanzaManager.IsBalanzaOpen = true;
        }

        public void CloseBalanzaAnimation()
        {
            _animator.Play("CloseBalanza");
            BalanzaManager.IsBalanzaOpen = false;
        }
    }
}
