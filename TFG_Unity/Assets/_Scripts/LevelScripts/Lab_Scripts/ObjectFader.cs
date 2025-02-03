using UnityEngine;

namespace _Scripts.LevelScripts.Lab_Scripts
{
    public class ObjectFader : MonoBehaviour
    {
        [SerializeField] private float _fadeSpeed = 10;
        [SerializeField] private float _fadeAmout = 0;
        [SerializeField] private SpriteRenderer[] spritesToFade;
        [SerializeField] private Color finalColor = Color.black;
        private Color _initialColor;
        private float _originalOpacity;

        private Renderer _renderer;
        private Material _material;
        public bool DoFade = false;
        
        private void Start()
        {
            _renderer = GetComponent<Renderer>();
            _material = _renderer.material;
            _originalOpacity = _material.color.a;
            _initialColor = _material.color;
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
            Color smoothColor = Color.Lerp(currentColor, new Color(finalColor.r, finalColor.g, finalColor.b, _fadeAmout), _fadeSpeed * Time.deltaTime);

            foreach (var sprite in spritesToFade)
            {
                sprite.color = Color.Lerp(sprite.color, new Color(finalColor.r, finalColor.g, finalColor.b, _fadeAmout), _fadeSpeed * Time.deltaTime);
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
            Color smoothColor = Color.Lerp(currentColor, _initialColor, _fadeSpeed * Time.deltaTime);

            foreach (var sprite in spritesToFade)
            {
                sprite.color = Color.Lerp(sprite.color, _initialColor, _fadeSpeed * Time.deltaTime);
            }

            _material.color = smoothColor;
        }
    }
}