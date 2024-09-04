using System;
using Unity.VisualScripting;
using UnityEngine;

namespace _Scripts.LevelScripts.Lab_Scripts
{
    public class CameraFadeObstacles : MonoBehaviour
    {
        private ObjectFader _fader;
        private GameObject player;

        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        /// <summary>
        /// Lanza un rayo desde la camara hasta el jugador, si colosiona con un obstaculo con un fader,
        /// establece su Fade a true para ocultarlo, sino, lo hace visible
        /// </summary>
        private void Update()
        {
            if (player.IsUnityNull())
            {
                return;
            }

            Vector3 rayDirection = player.transform.position - transform.position;
            Ray ray = new Ray(transform.position, rayDirection);

            if (Physics.Raycast(ray, out var hit))
            {
                if (hit.collider.IsUnityNull())
                {
                    return;
                }

                if (hit.collider.gameObject.CompareTag("Wall"))
                {
                    hit.collider.gameObject.TryGetComponent(out _fader);

                    if (!_fader.IsUnityNull())
                    {
                        _fader.DoFade = true;
                    }
                    return;
                }

                if (!_fader.IsUnityNull())
                {
                    _fader.DoFade = false;
                }
                
            }
        }
    }
}