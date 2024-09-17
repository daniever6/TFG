using System;
using UnityEngine;

namespace _Scripts.LevelScripts.Level_02
{
    public class OpenBalanza : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        private static bool _isOpen;

        private void OnMouseUp()
        {
            if (!_isOpen)
            {
                _animator.Play("OpenBalanza");
                _isOpen = true;
            }
            else
            {
                _animator.Play("CloseBalanza");
                _isOpen = false;
            }
        }
    }
}
