using _Scripts.Managers;
using _Scripts.Player;
using _Scripts.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using static LoginManager;

namespace _Scripts.DeathScene
{
    public class DeathSceneManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI deathReason;
        
        void Start()
        {
            deathReason.text = GameManager.PlayerDeathCause.DeathReason;
            StartCoroutine(SaveInDatabase());
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


                if (progresoData.result == "Ok" && progresoData.data.Count > 0)
                {
                    var level = GameManager.PlayerDeathCause.GameLevel;

                    switch (level)
                    {
                        case GameLevels.LevelRecibidor:
                            progresoData.data[0].MuertesVestirseMal = $"{Convert.ToInt32(progresoData.data[0].MuertesVestirseMal) + 1}";
                            dataRow = $@"""MuertesVestirseMal"" : ""{progresoData.data[0].MuertesVestirseMal}""";
                            break;

                        case GameLevels.Laboratory:
                            progresoData.data[0].MuertesNivelResiduos = $"{Convert.ToInt32(progresoData.data[0].MuertesNivelResiduos) + 1}";
                            dataRow = $@"""MuertesNivelResiduos"" : ""{progresoData.data[0].MuertesNivelResiduos}""";
                            break;

                        case GameLevels.LevelBases:
                            progresoData.data[0].MuertesNivelBase = $"{Convert.ToInt32(progresoData.data[0].MuertesNivelBase) + 1}";
                            dataRow = $@"""MuertesNivelBase"" : ""{progresoData.data[0].MuertesNivelBase}""";
                            break;

                        case GameLevels.LevelAcidos:
                            progresoData.data[0].MuertesNivelAcido = $"{Convert.ToInt32(progresoData.data[0].MuertesNivelAcido) + 1}";
                            dataRow = $@"""MuertesNivelAcido"" : ""{progresoData.data[0].MuertesNivelAcido}""";
                            break;

                        case GameLevels.LevelBalanza:
                            progresoData.data[0].MuertesNivelBalanza = $"{Convert.ToInt32(progresoData.data[0].MuertesNivelBalanza) + 1}";
                            dataRow = $@"""MuertesNivelBalanza"" : ""{progresoData.data[0].MuertesNivelBalanza}""";
                            break;

                        case GameLevels.LevelResiduos:
                            progresoData.data[0].MuertesNivelResiduos = $"{Convert.ToInt32(progresoData.data[0].MuertesNivelResiduos) + 1}";
                            dataRow = $@"""MuertesNivelResiduos"" : ""{progresoData.data[0].MuertesNivelResiduos}""";
                            break;
                    }

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

        /// <summary>
        /// Respuesta de la insercion de fila de progreso
        /// </summary>
        /// <param name="response"></param>
        void HandleDeleteProgressResponse(string response)
        {
            if (response != null)
            {
                Debug.Log("Tabla de progreso eliminada: " + response);
                StartCoroutine(DatabaseManager.DeleteData(newProgressRow, HandleProgresoResponse));
            }
            else
            {
                Debug.LogError("Error al eliminar progreso.");
            }
        }

        /// <summary>
        /// Permite el cambio de escena, diseñado para volver al menu principal
        /// </summary>
        /// <param name="sceneName">Nombre de la escena</param>
        public void ChangeScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        /// <summary>
        /// Reinicia el juego desde el último punto guardado antes de la muerte
        /// </summary>
        public void RepeatLevel()
        {
            var level = GameManager.PlayerDeathCause.GameLevel;

            switch (level)
            {
                case GameLevels.LevelRecibidor:
                    SceneManager.LoadScene("Level_00");
                    break;
                
                case GameLevels.Laboratory:
                    SceneManager.LoadScene("EscenaMainLevel_Gonzalo");
                    break;
                
                case GameLevels.LevelBases:
                    SceneManager.LoadScene("Level_01");
                    break;
                
                case GameLevels.LevelAcidos:
                    SceneManager.LoadScene("Level_02.1");
                    break;
                
                case GameLevels.LevelBalanza:
                    SceneManager.LoadScene("Level_02");
                    break;
                
                case GameLevels.LevelResiduos:
                    SceneManager.LoadScene("Level_03");
                    break;
            }
        }
    }
}
