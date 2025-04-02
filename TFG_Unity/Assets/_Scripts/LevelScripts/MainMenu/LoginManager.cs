using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField userInput;
    [SerializeField] private TMP_InputField edadInput;

    private bool userValid = false;
    private bool edadValid = false;

    [SerializeField] private Animator startButtonAnim;
    [SerializeField] private Button startButton;
    [SerializeField] private TextMeshProUGUI startButtonText;

    private Color startTextColor;

    [SerializeField] private GameObject MainCanvas;
    [SerializeField] private GameObject LoginCanvas;


    [SerializeField] private Material skinMaterial;
    [SerializeField] private Material hairMaterial;

    private void Start()
    {
        try
        {
            if(userInput != null) userInput.onValueChanged.AddListener(IsUserValid);
            if(edadInput != null) edadInput.onValueChanged.AddListener(IsEdadValid);

            startTextColor = startButtonText.color;

            CanStartLevel();
        }
        catch (Exception e) 
        {

        }
    }

    /// <summary>
    /// Quita los listeners
    /// </summary>
    private void OnDestroy()
    {
        if (userInput != null) userInput.onValueChanged.RemoveAllListeners();
        if (edadInput != null) edadInput.onValueChanged.RemoveAllListeners();
    }

    /// <summary>
    /// Comprueba si el nombre de usuario es valido
    /// </summary>
    /// <param name="name">Nombre de usuario</param>
    public void IsUserValid(string name)
    {
        userValid = !string.IsNullOrEmpty(name);

        CanStartLevel();
    }

    /// <summary>
    /// Comprueba si la edad es valida
    /// </summary>
    /// <param name="edad">Edad del usuario</param>
    public void IsEdadValid(string edad)
    {
        //Edad no es vacio
        if (string.IsNullOrWhiteSpace(edad))
        {
            edadValid = false;
            CanStartLevel();
            return;
        }

        //Edad no es un numero
        if (!int.TryParse(edad, out int edadNumero))
        {
            edadValid = false;
            CanStartLevel();
            return;
        }

        //Edad tiene que estar entre 0 y 100
        if (edadNumero < 0 || edadNumero > 100)
        {
            edadValid = false;
            CanStartLevel();
            return;
        }

        edadValid = true;

        CanStartLevel();
    }

    /// <summary>
    /// Activa el boton de empezar si la edad y el usuario es valido
    /// </summary>
    private void CanStartLevel()
    {
        try
        {
            if (edadValid && userValid)
            {
                startButton.enabled = true;
                startButtonAnim.enabled = true;
                startButtonText.color = startTextColor;
            }
            else
            {
                startButton.enabled = false;
                startButtonAnim.enabled = false;
                startButtonText.color = Color.gray;
            }
        }
        catch(Exception ex)
        {

        }
        
    }

    /// <summary>
    /// Cierra el menu de login
    /// </summary>
    public void BackToMenu()
    {
        MainCanvas.SetActive(true);
        LoginCanvas.SetActive(false);
    }

    /// <summary>
    /// Abre el menu de login
    /// </summary>
    public void OpenLoginMenu()
    {
        MainCanvas.SetActive(false);
        LoginCanvas.SetActive(true);
    }

    /// <summary>
    /// Guarda los datos del usuario, borra los datos de partidas anteriores y empieza la partida
    /// </summary>
    public void StartGame()
    {
        var savePath = Application.persistentDataPath + "/gameState.save";
        var clothPath = Application.persistentDataPath + "/clothingIndex.json";

        if (File.Exists(savePath))
            File.Delete(savePath);
        if (File.Exists(clothPath))
            File.Delete(clothPath);

        //Reseteamos los colores por defecto
        if (ColorUtility.TryParseHtmlString($"#EFC088", out Color color))
        {
            skinMaterial.color = color;
        }
        hairMaterial.color = Color.black;
        

        SceneManager.LoadScene("Intro");
    }
}
