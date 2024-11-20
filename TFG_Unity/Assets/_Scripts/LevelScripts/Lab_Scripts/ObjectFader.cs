using UnityEngine;

namespace _Scripts.LevelScripts.Lab_Scripts
{
    public class ObjectFader : MonoBehaviour
    {
        [SerializeField] private float _fadeSpeed = 10;
        [SerializeField] private float _fadeAmout = 0;
        [SerializeField] private SpriteRenderer[] spritesToFade;
        private float _originalOpacity;

        private Renderer _renderer;
        private Material _material;
        public bool DoFade = false;
        
        private void Start()
        {
            _renderer = GetComponent<Renderer>();
            _material = _renderer.material;
            _originalOpacity = _material.color.a;
        }

        private void Update()
        {
            if (DoFade)
            {
                FadeNow();
            }
            else
            {
                ResetFade();
            }
        }

        /// <summary>
        /// Reduce el alpha del color del material del objeto
        /// </summary>
        private void FadeNow()
        {
            Color currentColor = _material.color;
            Color smoothColor = new Color(currentColor.r, currentColor.g, currentColor.b,
                Mathf.Lerp(currentColor.a, _fadeAmout, _fadeSpeed * Time.deltaTime));

            foreach (var sprite in spritesToFade)
            {
                Color currentSpriteColor = sprite.color;
                Color smoothSpriteColor = new Color(currentSpriteColor.r, currentSpriteColor.g, currentSpriteColor.b,
                 Mathf.Lerp(currentSpriteColor.a, _fadeAmout, _fadeSpeed * Time.deltaTime));
                
                sprite.color = smoothSpriteColor;
            }

            _material.color = smoothColor;

        }

        /// <summary>
        /// Resetea el alpha del color del material del objeto
        /// a su valor original
        /// </summary>
        private void ResetFade()
        {
            Color currentColor = _material.color;
            Color smoothColor = new Color(currentColor.r, currentColor.g, currentColor.b,
                Mathf.Lerp(currentColor.a, _originalOpacity, _fadeSpeed * Time.deltaTime));

            foreach (var sprite in spritesToFade)
            {
                Color currentSpriteColor = sprite.color;
                Color smoothSpriteColor = new Color(currentSpriteColor.r, currentSpriteColor.g, currentSpriteColor.b,
                    Mathf.Lerp(currentSpriteColor.a, _originalOpacity, _fadeSpeed * Time.deltaTime));

                sprite.color = smoothSpriteColor;
            }

            _material.color = smoothColor;
        }
    }
}