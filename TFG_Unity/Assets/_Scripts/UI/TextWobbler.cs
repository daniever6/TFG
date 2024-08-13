using TMPro;
using UnityEngine;

namespace _Scripts.UI
{
    public class TextWobbler : MonoBehaviour
    {
        private TMP_Text textMesh;
        private Mesh mesh;
        private Vector3[] vertices;
        
        void Start()
        {
            textMesh = GetComponent<TMP_Text>();
        }

        // Update is called once per frame
        void Update()
        {
            textMesh.ForceMeshUpdate();
            mesh = textMesh.mesh;
            vertices = mesh.vertices;

            for (int i = 0; i < vertices.Length; i++)
            {
                Vector3 offset = Wobble(Time.time + i);

                vertices[i] += offset;
            }

            mesh.vertices = vertices;
            textMesh.canvasRenderer.SetMesh(mesh);
        }

        private Vector2 Wobble(float time)
        {
            return new Vector2(Mathf.Sin(time * 3.3f), Mathf.Cos(time * 2.5f));
        }
    }
}
