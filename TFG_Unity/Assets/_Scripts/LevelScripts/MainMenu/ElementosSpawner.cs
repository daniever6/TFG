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

            AdjustSpriteScale(spriteRectTransfrom);
            //spriteRectTransfrom.localScale /= Random.Range(1.5f, 4.5f);

            // Agregar un Rigidbody2D
            Rigidbody2D rb = spriteGameObject.AddComponent<Rigidbody2D>();
            rb.gravityScale = 1f;

            rb.AddTorque(Random.Range(-25f, 25f));


            // Establecer la posición
            spriteGameObject.transform.position = position;

            Destroy(spriteGameObject, 25f);

            yield return new WaitForSeconds(secondSpawn);
        }
    }

    private void AdjustSpriteScale(RectTransform spriteRectTransform)
    {
        // Porcentaje deseado del tamaño de la pantalla que quieres ocupar (3% en este caso)
        float desiredScreenPercentage = 0.25f; 

        // Obtener el tamaño de la pantalla en píxeles
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Obtener el tamaño original del sprite en píxeles (asumiendo que ya tiene un tamaño base)
        float spriteOriginalWidth = spriteRectTransform.rect.width;
        float spriteOriginalHeight = spriteRectTransform.rect.height;

        // Calcular el nuevo ancho y alto que debe tener el sprite para mantener el mismo porcentaje
        float targetWidth = screenWidth * desiredScreenPercentage;
        float targetHeight = screenHeight * desiredScreenPercentage;

        // Calcular el factor de escala necesario para alcanzar ese tamaño
        float scaleX = targetWidth / spriteOriginalWidth;
        float scaleY = targetHeight / spriteOriginalHeight;

        // Usar la escala más pequeña para mantener la proporción (por si la pantalla es más ancha o alta)
        float uniformScale = Mathf.Min(scaleX, scaleY);

        // Aplicar la escala al sprite
        spriteRectTransform.localScale = Vector3.one * uniformScale;

        // Si quieres un poco de variación aleatoria, como en tu ejemplo:
        spriteRectTransform.localScale /= (Random.Range(1.5f, 3f) * (screenWidth / 1920));
    }

}