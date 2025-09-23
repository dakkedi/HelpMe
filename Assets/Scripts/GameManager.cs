using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private Transform _playerSpawnPoint;
    [SerializeField] private bool _spawnEnemies = true;
    [SerializeField] private float _enemySpawnIntervalBase = 1f;

    public static GameManager Instance { get; private set; }

    private List<GameObject> _enemyList = new List<GameObject>();
    private float _nextSpawnTime;

    private void Awake() {
        Debug.Log("GameManager awake");
        if (Instance != null && Instance != this) {
            // If another GameManager exists and it's not this one, destroy this and do nothing.
            // The original one will handle scene initialization.
            Destroy(gameObject);
            return;
        } else {
            Instance = this;
            // DontDestroyOnLoad(this.gameObject); // This can cause issues with scene reloads. Let's manage state within the scene.
        }
        NewSpawnTime();
        InitScene();
    }

    private void InitScene() {
        Instantiate(_playerPrefab, _playerSpawnPoint.position, Quaternion.identity);
    }

    private void SpawnEnemy() {
        if (GridManager.Instance.TryGetRandomSpawnPoint(out Vector3 spawnPos))
        {
            GameObject enemy = Instantiate(_enemyPrefab, spawnPos, Quaternion.identity);
            _enemyList.Add(enemy);
        }
        else
        {
            Debug.LogWarning("Could not find a valid spawn position for an enemy on the grid's outer layers.");
        }
    }
    
    private void FixedUpdate() {
        if (_spawnEnemies && Time.time > _nextSpawnTime) {
            SpawnEnemy();
            NewSpawnTime();
        }
    }

    private void NewSpawnTime() {
        _nextSpawnTime = Time.time + _enemySpawnIntervalBase;
    }

    public void ReloadScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        _spawnEnemies = false;
    }

    public void ToggleSpawnEnemies() {
        _spawnEnemies = !_spawnEnemies;
    }

    public List<GameObject> GetCurrentEnemies() {
        return _enemyList;
    }

    public void RemoveEnemy(GameObject enemy) {
        if (_enemyList.Contains(enemy)) {
            _enemyList.Remove(enemy);
        }
    }
}
