using _Scripts.Managers;
using _Scripts.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using static LoginManager;

public class VictoryPanel : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(SaveInDatabase());
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

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


            if (progresoData.result == "Ok" && progresoData.data.Count > 0)
            {
                string newProgressRow = $@"
                {{
                        ""username"": ""{DatabaseConfig.DbUser}"",
                        ""token"": ""{PlayerPrefs.GetString("UserToken")}"",
                        ""table"": ""Puntuacion"",
                        ""data"": {{
                            ""Completado"" : ""1""
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
}
