using UnityEngine;

public class EmergencyLight : MonoBehaviour
{
    [SerializeField] private Vector3 rotationSpeed = new Vector3(25, 0, 0);

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}
