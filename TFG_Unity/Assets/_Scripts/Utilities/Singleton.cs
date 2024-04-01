﻿using UnityEngine;

namespace _Scripts.Utilities
{
    public abstract class StaticInstance<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance { get; private set; }
        protected virtual void Awake()=>Instance = this as T;

        protected void OnApplicationQuit()
        {
            Instance = null;
            Destroy(gameObject);
        }
    }

    public abstract class Singleton<T> : StaticInstance<T> where T : MonoBehaviour
    {
        protected override void Awake()
        {
            if(Instance != null) Destroy(gameObject);
            base.Awake();
        }
    }
}