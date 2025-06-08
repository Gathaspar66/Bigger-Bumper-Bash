using UnityEngine;

public class LevelConfigLoader : MonoBehaviour
{
    public static LevelConfigLoader instance;

    [Header("Config JSON file (drag & drop in Inspector)")]
    public TextAsset configJson;

    public GameConfig gameConfig;
    private bool isLoaded = false;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        LoadConfigFromTextAsset();
    }

    private void LoadConfigFromTextAsset()
    {
        if (configJson == null)
        {
            Debug.LogError("Config JSON file not assigned in Inspector!");
            return;
        }

        gameConfig = JsonUtility.FromJson<GameConfig>(configJson.text);
        isLoaded = true;
        Debug.Log("Config loaded from TextAsset. Levels count: " + gameConfig.levels.Length);
    }

    public bool IsConfigLoaded()
    {
        return isLoaded;
    }
}