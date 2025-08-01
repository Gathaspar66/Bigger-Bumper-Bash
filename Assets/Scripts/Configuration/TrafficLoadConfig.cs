using UnityEngine;

public class LevelConfigLoader : MonoBehaviour
{
    public static LevelConfigLoader instance;

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