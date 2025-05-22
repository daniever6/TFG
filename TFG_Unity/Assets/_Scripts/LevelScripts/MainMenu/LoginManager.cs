using System;
using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField userInput;
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private TMP_InputField edadInput;

    private bool userValid = false;
    private bool edadValid = false;
    private bool nameValid = false;

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
            if(nameInput != null) nameInput.onValueChanged.AddListener(IsNameValid);
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
    /// Comprueba si el nombre del jugador es valido
    /// </summary>
    /// <param name="name">Nombre del jugador</param>
    public void IsNameValid(string name)
    {
        nameValid = !string.IsNullOrEmpty(name);

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
            if (edadValid && userValid && nameValid)
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
        

        //StartCoroutine(DatabaseManager.GetData(CheckUserJson, HandleCheckUserResponse));
        StartCoroutine(RequestToken());
    }


    [System.Serializable]
    public class TokenResponse
    {
        public string result;
        public string token;
        public string until;
    }

    private IEnumerator RequestToken()
    {
        string jsonRequest = $@"
        {{
            ""username"": ""{DatabaseConfig.DbUser}"",
            ""password"": ""{DatabaseConfig.DbPassword}""
        }}";

        using (UnityWebRequest webRequest = new UnityWebRequest("https://tfvj.etsii.urjc.es/rest/login", "POST"))
        {
            byte[] jsonData = System.Text.Encoding.UTF8.GetBytes(jsonRequest);
            webRequest.uploadHandler = new UploadHandlerRaw(jsonData);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                string response = webRequest.downloadHandler.text;
                Debug.Log("Respuesta del servidor: " + response);

                TokenResponse tokenResponse = JsonUtility.FromJson<TokenResponse>(response);

                if (tokenResponse.result == "Ok")
                {
                    PlayerPrefs.SetString("UserToken", tokenResponse.token);
                    PlayerPrefs.Save();
                    Debug.Log("Token guardado: " + tokenResponse.token);

                    string CheckUserJson = $@"
                    {{
                        ""username"": ""{DatabaseConfig.DbUser}"",
                        ""password"": ""{DatabaseConfig.DbPassword}"",
                        ""table"": ""Usuarios"",
                        ""filter"": {{
                            ""Usuario"": ""{userInput.text}""
                        }}
                    }}";

                    StartCoroutine(DatabaseManager.GetData(CheckUserJson, HandleCheckUserResponse));
                }
                else
                {
                    Debug.LogError("Error en autenticación: " + response);
                }
            }
            else
            {
                Debug.LogError("Error en la solicitud: " + webRequest.error);
            }
        }
    }

    /// <summary>
    /// Comprueba si existe el usuario en la BD
    /// </summary>
    /// <param name="response"></param>
    void HandleCheckUserResponse(string response)
    {
        if (response != null)
        {
            // Convertimos la respuesta JSON en un objeto
            UserCheckResponse jsonResponse = JsonUtility.FromJson<UserCheckResponse>(response);

            if (jsonResponse.result == "Ok" && jsonResponse.data.Length > 0)
            {
                //Si ya existe, actualiza el usuario
                PlayerPrefs.SetString("NombreUsuario", userInput.text);
                PlayerPrefs.Save();

                string UserUpdateJson = $@"
                {{
                    ""username"": ""{DatabaseConfig.DbUser}"",
                    ""token"": ""{PlayerPrefs.GetString("UserToken")}"",
                    ""table"": ""Usuarios"",
                    ""data"": {{
                        ""Nombre"": ""{nameInput.text}"",
                        ""Edad"": {Convert.ToInt32(edadInput.text)}
                    }},
                    ""filter"": {{
                        ""Usuario"": ""{PlayerPrefs.GetString("NombreUsuario")}""
                    }}
                }}";

                StartCoroutine(DatabaseManager.UpdateData(UserUpdateJson, HandleUserUpdateResponse));
                return;
            }
        }

        // Si el usuario NO existe, lo insertamos
        InsertNewUser();
    }

    /// <summary>
    /// Lleva la logica de respuesta de la actualizacion del usuario
    /// </summary>
    /// <param name="response"></param>
    void HandleUserUpdateResponse(string response)
    {
        if (response != null)
        {
            Debug.Log("Usuario actualizado correctamente: " + response);
            SceneManager.LoadScene("Intro");
        }
        else
        {
            Debug.LogError("Error al actualizar el usuario.");
        }
    }

    /// <summary>
    /// Guarda el usuario en la base de datos
    /// </summary>
    public void InsertNewUser()
    {
        string UserJson = $@"
        {{
            ""username"": ""{DatabaseConfig.DbUser}"",
            ""password"": ""{DatabaseConfig.DbPassword}"",
            ""table"": ""Usuarios"",
            ""data"": {{
                ""Usuario"": ""{userInput.text}"",
                ""Nombre"": ""{nameInput.text}"",
                ""Edad"": {edadInput.text}
            }}
        }}";

        PlayerPrefs.SetString("NombreUsuario", userInput.text);
        PlayerPrefs.Save();

        StartCoroutine(DatabaseManager.InsertData(UserJson, HandleUserResponse));
    }

    /// <summary>
    /// Inserta un nuevo progreso en la base de datos
    /// </summary>
    /// <param name="response"></param>
    void HandleUserResponse(string response)
    {
        if (response != null)
        {
            string ProgressJson = $@"
            {{
                ""username"": ""{DatabaseConfig.DbUser}"",
                ""password"": ""{DatabaseConfig.DbPassword}"",
                ""table"": ""Puntuacion"",
                ""data"": {{
                    ""Usuario"": ""{userInput.text}"",
                    ""Completado"": {Convert.ToInt32(false)},
                    ""MuertesVestirseMal"": ""0"",
                    ""TiempoNivelBases"": ""0"",
                    ""NivelBasesCompletado"": ""0"",
                    ""MuertesNivelBase"": ""0"",
                    ""TiempoNivelAcido"": ""0"",
                    ""NivelAcidoCompletado"": ""0"",
                    ""MuertesNivelAcido"": ""0"",
                    ""TiempoNivelBalanza"": ""0"",
                    ""NivelBalanzaCompletado"": ""0"",
                    ""MuertesNivelBalanza"": ""0"",
                    ""TiempoNivelResiduos"": ""0"",
                    ""NivelResiduosCompletado"": ""0"",
                    ""MuertesNivelResiduos"": ""0""
                }}
            }}";

            StartCoroutine(DatabaseManager.InsertData(ProgressJson, HandleProgresoResponse));
            Debug.Log("Respuesta user de insercion: " + response);
        }
        else
        {
            Debug.LogError("Error al insertar.");
        }
    }

    /// <summary>
    /// Respuesta de la insercion de fila de progreso
    /// </summary>
    /// <param name="response"></param>
    void HandleProgresoResponse(string response)
    {
        if (response != null)
        {
            Debug.Log("Respuesta de progreso de insertar: " + response);
            SceneManager.LoadScene("Intro");
        }
        else
        {
            Debug.LogError("Error al insertar.");
        }
    }

    [System.Serializable]
    public class UserCheckResponse
    {
        public string result;
        public string[] data;
    }
}
