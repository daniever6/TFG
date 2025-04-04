using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System.Threading.Tasks;

public static class DatabaseManager
{
    /// <summary>
    /// Método para insertar datos en la base de datos
    /// </summary>
    /// <param name="json"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    public static IEnumerator InsertData(string json, System.Action<string> callback)
    {
        string url = DatabaseConfig.InsertUrl;

        using (UnityWebRequest webRequest = new UnityWebRequest(url, "POST"))
        {
            byte[] jsonData = Encoding.UTF8.GetBytes(json);
            webRequest.uploadHandler = new UploadHandlerRaw(jsonData);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");

            // Esperar respuesta del servidor
            yield return webRequest.SendWebRequest();

            // Verificar si hubo un error
            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                string response = webRequest.downloadHandler.text;
                callback(response); // Devolver la respuesta del servidor
            }
            else
            {
                Debug.LogError("Error en la inserción: " + webRequest.error);
                callback(null); // Devolver null en caso de error
            }
        }
    }

    /// <summary>
    /// Método para consultar datos de la base de datos
    /// </summary>
    /// <param name="json"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    public static IEnumerator GetData(string json, System.Action<string> callback)
    {
        string url = DatabaseConfig.GetUrl;
        using (UnityWebRequest webRequest = new UnityWebRequest(url, "POST"))
        {
            byte[] jsonData = Encoding.UTF8.GetBytes(json);
            webRequest.uploadHandler = new UploadHandlerRaw(jsonData);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");

            // Esperar respuesta del servidor
            yield return webRequest.SendWebRequest();

            // Verificar si hubo un error
            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                string response = webRequest.downloadHandler.text;
                callback(response); // Devolver la respuesta del servidor
            }
            else
            {
                Debug.LogError("Error en la consulta: " + webRequest.error);
                callback(null); // Devolver null en caso de error
            }
        }
    }

    // Método para actualizar datos en la base de datos
    public static IEnumerator UpdateData(string json, System.Action<string> callback)
    {
        string url = DatabaseConfig.UpdateUrl;
        using (UnityWebRequest webRequest = new UnityWebRequest(url, "POST"))
        {
            byte[] jsonData = Encoding.UTF8.GetBytes(json);
            webRequest.uploadHandler = new UploadHandlerRaw(jsonData);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");

            // Esperar respuesta del servidor
            yield return webRequest.SendWebRequest();

            // Verificar si hubo un error
            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                string response = webRequest.downloadHandler.text;
                callback(response); // Devolver la respuesta del servidor
            }
            else
            {
                Debug.LogError("Error en la actualización: " + webRequest.error);
                callback(null); // Devolver null en caso de error
            }
        }
    }

    public static IEnumerator DeleteData(string json, System.Action<string> callback)
    {
        string url = DatabaseConfig.DeleteUrl; // Suponiendo que tienes una URL específica para eliminar datos
        using (UnityWebRequest webRequest = new UnityWebRequest(url, "DELETE"))
        {
            // Si necesitas enviar datos (por ejemplo, para identificar el recurso a eliminar), puedes hacer lo siguiente:
            byte[] jsonData = Encoding.UTF8.GetBytes(json);
            webRequest.uploadHandler = new UploadHandlerRaw(jsonData);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");

            // Esperar respuesta del servidor
            yield return webRequest.SendWebRequest();

            // Verificar si hubo un error
            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                string response = webRequest.downloadHandler.text;
                callback(response); // Devolver la respuesta del servidor
            }
            else
            {
                Debug.LogError("Error al eliminar: " + webRequest.error);
                callback(null); // Devolver null en caso de error
            }
        }
    }


    [System.Serializable]
    private class TokenResponse
    {
        public string result;
        public string token;
        public string until;
    }
}
