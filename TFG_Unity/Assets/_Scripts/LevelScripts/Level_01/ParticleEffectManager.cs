using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Scripts.LevelScripts.Level_01
{
    [Serializable]
    public class ParticlePrefab
    {
        public string Key;
        public GameObject Value;
        
        public ParticlePrefab(string key, GameObject value)
        {
            Key = key;
            Value = value;
        }
    }
    
    [Serializable]
    public class SerializableDictionary
    {
        [SerializeField] private List<ParticlePrefab> items = new();

        public GameObject this[string key]
        {
            get => items.Where(p => p.Key == key).First().Value;
        }

        public bool Contains(string key)
        {
            return items.Any(p => p.Key == key);
        }
    }
    
    public class ParticleEffectManager : MonoBehaviour
    {
        [SerializeField] private SerializableDictionary particles = new(); //Diccionario de prefabs de particulas
        [SerializeField] private Transform particlePosition; //Posicion en la que instanciar

        private static ParticleEffectManager _instance;
        public static ParticleEffectManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new();
                }

                return _instance;
            }
        }

        private void Start()
        {
            _instance = this;
        }

        /// <summary>
        /// Instancia un prefab de particulas en la posicion de particlePosition indicado
        /// </summary>
        /// <param name="particleName">Nombre del Key la particula</param>
        public void InstantiateParticle(string particleName)
        {
            if (!particles.Contains(particleName))
            {
                return;
            }
            
            var particle = particles[particleName];
            Instantiate(particle, particlePosition.position, Quaternion.LookRotation(Vector3.up), particlePosition);
        }
        
        /// <summary>
        /// Instancia un prefab de particulas en una transform indicada
        /// </summary>
        /// <param name="particleName">Nombre del Key de la particula</param>
        /// <param name="pos">Posicion en la que instanciar el gameobject</param>
        public void InstantiateParticleInPos(string particleName, Transform pos)
        {
            if (!particles.Contains(particleName))
            {
                return;
            }
            
            var particle = particles[particleName];
            Instantiate(particle, pos.position, Quaternion.LookRotation(Vector3.up), pos);
        }
    }
}