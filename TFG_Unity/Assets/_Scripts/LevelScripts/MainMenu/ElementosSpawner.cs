using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ElementosSpawner : MonoBehaviour
{
    [SerializeField] private Sprite[] spritePrefabs;  // Lista de sprites
    [SerializeField] private GameObject[] spawnPositions;
    [SerializeField] private GameObject SpriteParent;

    public float secondSpawn = 0.5f;
    public float minTras;
    public float maxTras;

    public int timeBeforeStart = 0;

    async void Start()
    {
        await Task.Delay(timeBeforeStart);
        StartCoroutine(SpriteSpawner());
    }

    IEnumerator SpriteSpawner()
    {
        while (true)
        {
            var position = spawnPositions[Random.Range(0, spawnPositions.Length)].transform.position;
            //var wanted = Random.Range(minTras, maxTras);
            //var position = new Vector3(wanted, SpriteParent.transform.position.y, SpriteParent.transform.position.z);

            // Seleccionamos el sprite aleatorio del arreglo de sprites
            var spriteSelected = spritePrefabs[Random.Range(0, spritePrefabs.Length)];

            // Crear un nuevo GameObject vacío para este sprite
            GameObject spriteGameObject = new GameObject();

            spriteGameObject.transform.parent = SpriteParent.transform;

            // Agregar un SpriteRenderer y asignar el sprite
            Image spriteImageComponent = spriteGameObject.AddComponent<Image>();
            spriteImageComponent.sprite = spriteSelected;

            // Dimensiones del sprite
            RectTransform spriteRectTransfrom = spriteGameObject.GetComponent<RectTransform>();
            spriteRectTransfrom.localScale /= Random.Range(1.5f, 4.5f);

            // Agregar un Rigidbody2D
            Rigidbody2D rb = spriteGameObject.AddComponent<Rigidbody2D>();
            rb.gravityScale = 1f;

            rb.AddTorque(Random.Range(-25f, 25f));


            // Establecer la posición
            spriteGameObject.transform.position = position;

            Destroy(spriteGameObject, 15f);

            yield return new WaitForSeconds(secondSpawn);
        }
    }
}