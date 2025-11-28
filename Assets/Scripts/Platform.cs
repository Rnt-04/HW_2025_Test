using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class Platform : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float minLife = 4f;
    [SerializeField] private float maxLife = 5f;
    [SerializeField] private float spawnThreshold = 3f; // When to spawn next platform
    [SerializeField] private float distanceToNext = 3f;

    [Header("UI")]
    [SerializeField] private TMP_Text timerText;

    private float lifeTimer;
    private bool hasSpawnedNext = false;
    private bool hasGivenScore = false;

    [HideInInspector] public Vector3 entryDirection = Vector3.zero;

    private static readonly Vector3[] directions = { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };

    // Static list to track active platforms
    private static List<Platform> activePlatforms = new List<Platform>();

    private void Start()
    {
        if (GameDataManager.Instance != null)
        {
            minLife = GameDataManager.Instance.minDestroyTime;
            maxLife = GameDataManager.Instance.maxDestroyTime;
            spawnThreshold = GameDataManager.Instance.spawnTime;
        }
        
        lifeTimer = Random.Range(minLife, maxLife);

        // Safety
        if (spawnThreshold >= lifeTimer)
            spawnThreshold = lifeTimer - 0.1f;

        timerText ??= GetComponentInChildren<TMP_Text>();

        activePlatforms.Add(this);
    }

    private void Update()
    {
        lifeTimer -= Time.deltaTime;
        lifeTimer = Mathf.Max(lifeTimer, 0f);

        if (timerText != null)
            timerText.text = lifeTimer.ToString("F1");

        // Spawn next platform only if < 2 active platforms
        if (!hasSpawnedNext && lifeTimer <= spawnThreshold && activePlatforms.Count < 2)
        {
            SpawnNextPlatform();
            hasSpawnedNext = true;
        }

        if (lifeTimer <= 0f)
        {
            activePlatforms.Remove(this);
            Destroy(gameObject);
        }
    }

    private void SpawnNextPlatform()
    {
        Vector3 dir = GetRandomDirection();
        Vector3 spawnPos = transform.position + dir * distanceToNext;

        GameObject newObj = Instantiate(gameObject, spawnPos, Quaternion.identity);
        Platform newPlatform = newObj.GetComponent<Platform>();
        newPlatform.entryDirection = dir;
        newPlatform.ResetPlatform();
    }

    private Vector3 GetRandomDirection()
    {
        List<Vector3> validDirs = new List<Vector3>();
        foreach (var dir in directions)
        {
            if (dir != -entryDirection)
                validDirs.Add(dir);
        }
        return validDirs[Random.Range(0, validDirs.Count)];
    }

    public void ResetPlatform()
    {
        hasSpawnedNext = false;
        hasGivenScore = false;
        lifeTimer = Random.Range(minLife, maxLife);
        spawnThreshold = Mathf.Min(spawnThreshold, lifeTimer - 0.1f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasGivenScore) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.OnScorePlatform();
                hasGivenScore = true;
            }
        }
    }
}
