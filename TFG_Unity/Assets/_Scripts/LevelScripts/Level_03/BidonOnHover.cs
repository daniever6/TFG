using System;
using DG.Tweening;
using UnityEngine;

namespace _Scripts.LevelScripts.Level_03
{
    public class BidonOnHover : MonoBehaviour
    {
        [SerializeField] private GameObject tapa;

        private Vector3 _tapaInitialPos;
        private Vector3 _aditionalMovement = new Vector3(0, 0.3f, 0);
        private bool isOpen;

        private void Start()
        {
            _tapaInitialPos = tapa.transform.position;
        }

        public void OpenContenedor()
        {
            if(isOpen) return;
            
            isOpen = true;
            
            tapa.transform.DORotate(new Vector3(0, 0, 720), 1, RotateMode.LocalAxisAdd)
                .SetEase(Ease.InOutSine);

            tapa.transform.DOMoveY(_tapaInitialPos.y + _aditionalMovement.y, 1)
                .SetEase(Ease.InOutSine);
        }

        public void CloseContenedor()
        {
            if(!isOpen) return;
            
            isOpen = false;
            
            tapa.transform.DORotate(new Vector3(0, 0, -720), 1, RotateMode.LocalAxisAdd)
                .SetEase(Ease.InOutSine);

            tapa.transform.DOMoveY(_tapaInitialPos.y, 1)
                .SetEase(Ease.InOutSine);
        }
    }
}
