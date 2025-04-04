
using UnityEngine;

public static class DatabaseConfig
{
    private static string dbUser;
    private static string dbPassword;
    private static string insertUrl;
    private static string getUrl;
    private static string updateUrl;
    private static string deleteUrl;

    /// <summary>
    /// Usuario de acceso a la base de datos
    /// </summary>
    public static string DbUser
    {
        get
        {
            if (string.IsNullOrEmpty(dbUser))
            {
                LoadConfig();
            }
            return dbUser;
        }
    }

    /// <summary>
    /// Constraseña de acceso a la base de datos
    /// </summary>
    public static string DbPassword
    {
        get
        {
            if (string.IsNullOrEmpty(dbPassword))
            {
                LoadConfig();
            }
            return dbPassword;
        }
    }

    /// <summary>
    /// URL de insercion
    /// </summary>
    public static string InsertUrl
    {
        get
        {
            if (string.IsNullOrEmpty(insertUrl))
            {
                LoadConfig();
            }
            return insertUrl;
        }
    }

    /// <summary>
    /// URL para los GET
    /// </summary>
    public static string GetUrl
    {
        get
        {
            if (string.IsNullOrEmpty(getUrl))
            {
                LoadConfig();
            }
            return getUrl;
        }
    }

    /// <summary>
    /// URL de actualizacion
    /// </summary>
    public static string UpdateUrl
    {
        get
        {
            if (string.IsNullOrEmpty(updateUrl))
            {
                LoadConfig();
            }
            return updateUrl;
        }
    }

    public static string DeleteUrl
    {
        get 
        {
            if (string.IsNullOrEmpty(deleteUrl))
            {
                LoadConfig();
            }
            return deleteUrl;
        }
    }


    /// <summary>
    /// Carga los datos de configuracion de la base de datos
    /// </summary>
    private static void LoadConfig()
    {
        // Cargar el archivo desde Resources
        TextAsset configText = Resources.Load<TextAsset>("DatabaseKeys");

        if (configText != null)
        {
            // Deserializar el archivo JSON a la clase ConfigData
            ConfigData config = JsonUtility.FromJson<ConfigData>(configText.text);
            dbUser = config.dbUser;
            dbPassword = config.dbPassword;
            insertUrl = config.insertUrl;
            getUrl = config.getUrl;
            updateUrl = config.updateUrl;
            deleteUrl = config.deleteUrl;
        }
        else
        {
            Debug.LogError("No se encontró el archivo config.json en Resources.");
        }
    }

    [System.Serializable]
    public class ConfigData
    {
        public string dbUser;
        public string dbPassword;
        public string insertUrl;
        public string getUrl;
        public string updateUrl;
        public string deleteUrl;
    }
}
