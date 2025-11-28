using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance;

    public float playerSpeed = 3f;

    [Header("Platform Settings")]
    public float minDestroyTime = 4f;
    public float maxDestroyTime = 5f;
    public float spawnTime = 2.5f;

    [Header("JSON Config")]
    public string jsonFileName = "config";

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        LoadJsonConfig();
    }

    private void LoadJsonConfig()
    {
        TextAsset file = Resources.Load<TextAsset>(jsonFileName);

        if (file == null)
        {
            Debug.LogWarning("JSON config not found. Using default values.");
            return;
        }

        try
        {
            GameConfig config = JsonUtility.FromJson<GameConfig>(file.text);

            if (config != null)
            {
                playerSpeed = config.player_data.speed;

                minDestroyTime = config.pulpit_data.min_pulpit_destroy_time;
                maxDestroyTime = config.pulpit_data.max_pulpit_destroy_time;
                spawnTime = config.pulpit_data.pulpit_spawn_time;
            }
        }
        catch
        {
            Debug.LogError("Error parsing JSON config. Using defaults.");
        }
    }
}
