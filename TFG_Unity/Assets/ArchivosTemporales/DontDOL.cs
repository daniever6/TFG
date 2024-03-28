using UnityEngine;

namespace ArchivosTemporales
{
    public class DontDOL : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
