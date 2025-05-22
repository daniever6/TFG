using _Scripts.Utilities;
using System;
using UnityEngine;
using static LoginManager;
using UnityEngine.Networking;
using System.Collections;
using _Scripts.Managers;
using System.Collections.Generic;

public class LevelDBManager : GameplayMonoBehaviour<LevelDBManager>
{
    public static Action OnStartTimeCount;
    public static Action OnLevelCompleted;

    public static float timer = 0f;
    public static bool isTiming = false;

    [SerializeField] private string levelName;
    private bool completed = false;

    void Update()
    {
        if (isTiming)
        {
            timer += Time.deltaTime;
        }
    }

    private void OnEnable()
    {
        OnStartTimeCount += StartCounter;
        OnLevelCompleted += LevelCompleted;
    }

    private void OnDisable()
    {
        OnStartTimeCount -= StartCounter;
        OnLevelCompleted -= LevelCompleted;
    }

    /// <summary>
    /// Comienza el contador del nivel
    /// </summary>
    private void StartCounter()
    {
        isTiming = true;
    }

    /// <summary>
    /// Metodo para guardar en la BD
    /// </summary>
    /// <returns></returns>
    public IEnumerator SaveInDatabase()
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
                            ""table"": ""Puntuacion"",
                            ""filter"": {{
                                ""Usuario"": ""{PlayerPrefs.GetString("NombreUsuario")}""
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

    private static string dataRow = string.Empty;
    private static string newProgressRow = string.Empty;

    /// <summary>
    /// Respuesta del get de usuario
    /// </summary>
    /// <param name="response"></param>
    void HandleCheckUserResponse(string response)
    {
        if (response != null)
        {
            // Convertimos la respuesta JSON en un objeto
            Response progresoData = JsonUtility.FromJson<Response>(response);

            int minutes = Mathf.FloorToInt(timer / 60f);
            int seconds = Mathf.FloorToInt(timer % 60f);
            string formattedTime = string.Format("{0:00}:{1:00}", minutes, seconds);

            if (progresoData.result == "Ok" && progresoData.data.Count > 0)
            {
                var level = GameManager.PlayerDeathCause.GameLevel;


                dataRow = $@"""TiempoNivel{levelName}"": ""{formattedTime}""";

                newProgressRow = $@"
                    {{
                            ""username"": ""{DatabaseConfig.DbUser}"",
                            ""token"": ""{PlayerPrefs.GetString("UserToken")}"",
                            ""table"": ""Puntuacion"",
                            ""data"": {{
                                {dataRow}
                            }},
                            ""filter"": {{
                                ""Usuario"": ""{PlayerPrefs.GetString("NombreUsuario")}""
                            }}
                    }}";

                StartCoroutine(DatabaseManager.UpdateData(newProgressRow, HandleProgresoResponse));

                dataRow = $@"""Nivel{levelName}Completado"": 1";

                newProgressRow = $@"
                    {{
                            ""username"": ""{DatabaseConfig.DbUser}"",
                            ""token"": ""{PlayerPrefs.GetString("UserToken")}"",
                            ""table"": ""Puntuacion"",
                            ""data"": {{
                                {dataRow}
                            }},
                            ""filter"": {{
                                ""Usuario"": ""{PlayerPrefs.GetString("NombreUsuario")}""
                            }}
                    }}";

                StartCoroutine(DatabaseManager.UpdateData(newProgressRow, HandleProgresoResponse));

                return;
            }
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
            Debug.Log("Tabla de progreso actualizada: " + response);
            timer = 0f;
            completed = false;
        }
        else
        {
            Debug.LogError("Error al actualizar progreso.");
        }
    }

    [System.Serializable]
    public class Response
    {
        public string result;
        public List<Data> data;  // Lista para manejar múltiples elementos en el array "data"
    }

    [System.Serializable]
    public class Data
    {
        public string Usuario;
        public int Completado;
        public string MuertesVestirseMal;
        public string MuertesNivelBase;
        public string MuertesNivelAcido;
        public string MuertesNivelBalanza;
        public string MuertesNivelResiduos;
    }

    /// <summary>
    /// Termina el tiempo del contador y lo sube a la BD
    /// </summary>
    private void LevelCompleted()
    {
        isTiming = false;
        completed = true;

        //Subir a la BD
        StartCoroutine(SaveInDatabase());
    }

    /// <summary>
    /// Reanuda el contador despues de la pausa
    /// </summary>
    protected override void OnPostResumed()
    {
        base.OnPostResumed();
        isTiming = true;
    }

    /// <summary>
    /// Pausa el contador cuando se pausa el juego
    /// </summary>
    protected override void OnPostPaused()
    {
        base.OnPostPaused();
        isTiming = false;
    }
}
