using System;
using _Scripts.Managers;
using UnityEngine;

namespace _Scripts.UI
{
    public class DangerWarning : MonoBehaviour
    {
        [SerializeField] private GameObject warningImage;
        [SerializeField] private Animation dangerAnim;
        
        private void Awake()
        {
            warningImage.SetActive(false);
            CombinationsManager.CombinationErrorEvent += DoWarningAnimation;
        }

        public void DoWarningAnimation()
        {
            warningImage.SetActive(true);
            dangerAnim.Play();
        }

        public void DesactivateAnimation()
        {
            warningImage.SetActive(false);
            dangerAnim.Stop();
        }
    }
}
