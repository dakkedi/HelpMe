using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private Transform _playerSpawnPoint;
    [SerializeField] private bool _spawnEnemies = false;
    [SerializeField] private float _enemySpawnRadius = 15f;
    [SerializeField] private float _enemySpawnIntervalBase = 1f;

    public static GameManager Instance { get; private set; }

    private List<GameObject> _enemyList = new List<GameObject>();
    private float _nextSpawnTime;

    private void Awake() {
        Debug.Log("GameManager awake");
        if (Instance != null && Instance != this) {
            Destroy(this.gameObject);
            Instance.InitScene();
        } else {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        NewSpawnTime();
        InitScene();
    }

    private void InitScene() {
        Instantiate(_playerPrefab, _playerSpawnPoint.position, Quaternion.identity);
        _spawnEnemies = true;
    }

    private void SpawnEnemy() {
        float randomAngle = Random.Range(0f, 2f * Mathf.PI);
        Vector3 spawnOffset = new Vector3(Mathf.Cos(randomAngle) * _enemySpawnRadius, Mathf.Sin(randomAngle) * _enemySpawnRadius, 0);
        GameObject enemey = Instantiate(_enemyPrefab, _playerSpawnPoint.position + spawnOffset, Quaternion.identity);
        _enemyList.Add(enemey);
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
