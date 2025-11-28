using UnityEngine;

public class ConfigManager : MonoBehaviour
{
    public static ConfigManager Instance { get; private set; }

    public GameConfig Config { get; private set; }

    public float PlayerSpeed => Config?.player_data?.speed ?? 3f;
    public float MinPlatformLife => Config?.pulpit_data?.min_pulpit_destroy_time ?? 4f;
    public float MaxPlatformLife => Config?.pulpit_data?.max_pulpit_destroy_time ?? 5f;
    public float PlatformSpawnInterval => Config?.pulpit_data?.pulpit_spawn_time ?? 2.5f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadConfig();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadConfig()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("game_config");

        if (jsonFile == null)
        {
            Debug.LogError("ConfigManager: game_config.json not found in Resources!");
            UseDefaults();
            return;
        }

        Config = JsonUtility.FromJson<GameConfig>(jsonFile.text);

        if (Config == null)
        {
            Debug.LogError("ConfigManager: failed to parse config JSON. Using defaults.");
            UseDefaults();
        }
    }

    private void UseDefaults()
    {
        Config = new GameConfig
        {
            player_data = new PlayerData { speed = 3f },
            pulpit_data = new PulpitData
            {
                min_pulpit_destroy_time = 4f,
                max_pulpit_destroy_time = 5f,
                pulpit_spawn_time = 2.5f
            }
        };
    }
}
