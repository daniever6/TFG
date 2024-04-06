using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    [AddComponentMenu("UI/Blur Panel")]
    public class BlurPanel : Image
    {
        [SerializeField] private bool animate;
        [SerializeField] private float time = 0.5f;
        [SerializeField] private float delay = 0f;

        private CanvasGroup _canvasGroup;

        protected override void Reset()
        {
            color = Color.black * 0.1f;
        }

        protected override void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        protected override void OnEnable()
        {
            if (Application.isPlaying)
            {
                material.SetFloat("_Size", 0);
                _canvasGroup.alpha = 0;
                LeanTween.value(gameObject, UpdateBlur, 0, 1, time).setDelay(delay);

            }
        }
        void UpdateBlur(float value)
        {
            material.SetFloat("_Size", value);
            _canvasGroup.alpha = value;
        }
    }
}
