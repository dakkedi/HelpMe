using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _coinPrefab;
    [SerializeField] private Transform _playerSpawnPoint;
    [SerializeField] private Slider _uiXpSlider;

    [SerializeField] private bool _gameHalted = false;
    [SerializeField] private bool _spawnEnemies = true;
    [SerializeField] private float _enemySpawnIntervalBase = 1f;
    [SerializeField] private int _startingCoin = 5;
    [SerializeField] private int _currentCoin = 0;

    [SerializeField] private TextMeshProUGUI _uiHp;
    [SerializeField] private TextMeshProUGUI _uiCoin;
    [SerializeField] private List<TowerController> _towers = new List<TowerController>();
    public void AddToTowers(TowerController tower)
    {
        _towers.Add(tower);
    }

    private GameObject _player;

    public static GameManager Instance { get; private set; }

    public event EventHandler OnXpCollect;

    private List<GameObject> _enemyList = new List<GameObject>();
    private float _nextSpawnTime;
    private float _waveStartTime;

    private void Awake()
    {
        Debug.Log("GameManager awake");
        if (Instance != null && Instance != this)
        {
            // If another GameManager exists and it's not this one, destroy this and do nothing.
            // The original one will handle scene initialization.
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
            InitScene();
            // DontDestroyOnLoad(this.gameObject); // This can cause issues with scene reloads. Let's manage state within the scene.
        }
    }

    private void Start()
    {
        // _player = Instantiate(_playerPrefab, _playerSpawnPoint.position, Quaternion.identity);
    }

    private void Update()
    {
        if (_spawnEnemies && Time.time > _nextSpawnTime)
        {
            SpawnEnemy();
            NewSpawnTime();
        }
    }

    private void InitScene()
    {
        _currentCoin = _startingCoin;
        UpdateCoinUi();
        NewSpawnTime();
        // Instantiate(_playerPrefab, _playerSpawnPoint.position, Quaternion.identity);
        _waveStartTime = Time.time;
    }

    private void NewSpawnTime()
    {
        _nextSpawnTime = Time.time + _enemySpawnIntervalBase;
    }

    public List<GameObject> GetCurrentEnemies()
    {
        return _enemyList;
    }

    private void SpawnEnemy()
    {
        // Increase spawn amount by 1 every 10 seconds.
        int enemiesToSpawn = 1 + Mathf.FloorToInt((Time.time - _waveStartTime) / 10f);

        for (int i = 0; i < enemiesToSpawn; i++)
        {
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
    }

    private void SpawnCoinOnTarget(Transform target)
    {
        Instantiate(_coinPrefab, target.position, Quaternion.identity);
    }

    public void RemoveEnemy(GameObject enemy)
    {
        if (_enemyList.Contains(enemy))
        {
            _enemyList.Remove(enemy);
            SpawnCoinOnTarget(enemy.transform);
        }
    }

    public void coinCollected()
    {
        _currentCoin++;
        OnXpCollect?.Invoke(this, EventArgs.Empty);
        UpdateCoinUi();
    }

    public int GetCurrentCoin()
    {
        return _currentCoin;
    }

    public void UseCoin(int cost)
    {
        _currentCoin -= cost;
        UpdateCoinUi();
    }

    private void UpdateCoinUi()
    {
        _uiCoin.text = "Coin: " + _currentCoin.ToString();
    }

    public void UpdateHpUi(int hp)
    {
        _uiHp.text = "HP: " + hp.ToString();
    }

    public void HaltGame(bool halt)
    {
        Debug.Log("Pausing game");
        Time.timeScale = halt ? 0f : 1f;
        _gameHalted = halt;
    }

    public bool IsGameHalted()
    {
        return _gameHalted;
    }

    public GameObject GetPlayer()
    {
        return _player;
    }
    public Slider GetXpSlider()
    {
        return _uiXpSlider;
    }
}
